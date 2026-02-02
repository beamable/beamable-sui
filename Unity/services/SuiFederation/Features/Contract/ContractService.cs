using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beamable.Common.Api.Auth;
using Beamable.Common.Content;
using Beamable.Common.Inventory;
using Beamable.Server;
using Beamable.SuiFederation.Features.LockManager;
using Beamable.Server.Content;
using Beamable.SuiFederation.Caching;
using Beamable.SuiFederation.Features.Contract.Exceptions;
using Beamable.SuiFederation.Features.Contract.Handlers;
using Beamable.SuiFederation.Features.Contract.Storage;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Contract.SuiClientWrapper;
using SuiFederationCommon;
using Beamable.SuiFederation.Features.Contract.Models;
using Beamable.Common; //DON'T REMOVE THIS LINE, NEEDED FOR SubscribeContentUpdateEvent
using Beamable.SuiFederation.Extensions;
using SuiFederationCommon.Extensions;
using SuiFederationCommon.FederationContent; //DON'T REMOVE THIS LINE, NEEDED FOR SubscribeContentUpdateEvent
using SuiFederationCommon.Models; //DON'T REMOVE THIS LINE, NEEDED FOR SubscribeContentUpdateEvent

namespace Beamable.SuiFederation.Features.Contract;

public class ContractService : IService
{
    private readonly LockManagerService _lockManagerService;
    private readonly ContentService _contentService;
    private readonly ContentContractHandlerResolver _contractHandlerResolver;
    private readonly ContractCollection _contractCollection;
    private readonly SuiClient _suiClient;
    public ContractService(SocketRequesterContext socketRequesterContext, LockManagerService lockManagerService, ContentService contentService, ContentContractHandlerResolver contentContractHandlerResolver, ContractCollection contractCollection, SuiClient suiClient)
    {
        _lockManagerService = lockManagerService;
        _contentService = contentService;
        _contractHandlerResolver = contentContractHandlerResolver;
        _contractCollection = contractCollection;
        _suiClient = suiClient;
        SubscribeContentUpdateEvent(socketRequesterContext);
    }

    private void SubscribeContentUpdateEvent(SocketRequesterContext socketRequesterContext)
    {
#if !DEBUG
        socketRequesterContext.Subscribe<object>(Constants.Features.Services.CONTENT_UPDATE_EVENT, _ =>
        {
            Task.Run(async () =>
            {
                try
                {
                    BeamableLogger.Log("Content was updated");
                    GlobalCache.Remove(nameof(FetchFederationContentLocal));
                    await InitializeContentContracts();
                }
                catch (Exception ex)
                {
                    BeamableLogger.LogError($"Error during content update: {ex}");
                }
            });
        });
#endif
    }

    public async Task InitializeContentContracts()
    {
        const string lockName = "initializeContracts";
        try
        {
            _ = Extensions.TaskExtensions.RunAsyncBlock(async () =>
            {
                if (!await _lockManagerService.AcquireLock(lockName)) return;

                await _suiClient.Initialize();

                foreach (var model in await FetchFederationContentForContracts())
                {
                    var handler = _contractHandlerResolver.Resolve(model);
                    await handler.HandleContract(model);
                }

                BeamableLogger.Log("Done with regular contracts, starting kiosks...");

                foreach (var model in await FetchFederationContentForKioskContracts())
                {
                    await _contractHandlerResolver.HandleContent(model);
                }

                await _contractHandlerResolver.HandlePersonalKiosk();

                BeamableLogger.Log("All contracts initialized.");
            });
        }
        finally
        {
            await _lockManagerService.ReleaseLock(lockName);
        }
    }

    public async Task<TContract> GetByContentId<TContract>(string contentId) where TContract : ContractBase
    {
        var contract = await _contractCollection.GetByContentId<TContract>(contentId);
        if (contract is null)
            throw new ContractException($"Contract for {contentId} don't exist.");
        return contract;
    }

    public async Task<TContract?> GetByContent<TContract>(string contentId) where TContract : ContractBase
    {
        return await _contractCollection.GetByContentId<TContract>(contentId);
    }

    public async Task<KioskContract> GetKioskContract(string itemType, string priceContentId)
    {
        var contract = await _contractCollection.GetKioskContract(itemType, priceContentId);
        if (contract is null)
            throw new ContractException($"Kiosk contract for {itemType} and {priceContentId} don't exist.");
        return contract;
    }

    public async Task<bool> UpsertContract<TContract>(TContract contract, string id) where TContract : ContractBase
    {
        return await _contractCollection.TryUpsert(contract, id);
    }

    public async IAsyncEnumerable<IContentObject> FetchFederationContent(ExternalIdentity externalIdentity)
    {
        var federationContent = await FetchFederationContentLocal();
        foreach (var content in federationContent)
        {
            if (externalIdentity is not null)
            {
                switch (content)
                {
                    case CurrencyContent coin when coin.federation.HasValue && coin.federation.Value.Namespace == externalIdentity.providerNamespace:
                    case ItemContent item when item.federation.HasValue && item.federation.Value.Namespace == externalIdentity.providerNamespace:
                        yield return content;
                        break;
                }
            }
            else
            {
                yield return content;
            }
        }
    }

    public async Task<IEnumerable<IContentObject>> FetchFederationContentForState(ExternalIdentity externalIdentity)
    {
        var federationContent = await FetchFederationContentLocal();
        var currencies = federationContent
            .Where(item => item is CurrencyContent coin && coin.federation.Matches(externalIdentity));

        var groupsByPath = federationContent
            .Where(item => item is ItemContent)
            .GroupBy(item => {
                var idParts = item.Id.Split('.');
                return string.Join(".", idParts.Take(idParts.Length - 1));
            })
            .ToDictionary(g => g.Key, g => g.ToList());

        var matchingItems = groupsByPath
            .Select(kvp => kvp.Value.FirstOrDefault(co => co is ItemContent item && item.federation.Matches(externalIdentity)))
            .Where(co => co is not null)
            .Cast<IContentObject>();
        return matchingItems.Concat(currencies);
    }

    public async Task<IEnumerable<ContentContractsModel>> FetchFederationContentForContracts()
    {
        var federationContent = await FetchFederationContentLocal();
        var currencies = federationContent.Where(item => item is CurrencyContent)
            .Select(item => new ContentContractsModel(item, []));

        var groupsByPath = federationContent
            .Where(item => item is ItemContent)
            .GroupBy(item => {
                var idParts = item.Id.Split('.');
                return string.Join(".", idParts.Take(idParts.Length - 1));
            })
            .ToDictionary(g => g.Key, g => g.ToList());

        var allPossibleModels = groupsByPath.Select(kvp =>
        {
            var parentPath = kvp.Key;
            var parentPathPartsCount = parentPath.Split('.').Length;
            var parentObject = kvp.Value.First();
            var children = groupsByPath
                .Where(childKvp =>
                    childKvp.Key.StartsWith(parentPath + ".") &&
                    childKvp.Key.Split('.').Length == parentPathPartsCount + 1
                )
                .Select(childGroup => childGroup.Value.First())
                .ToList();

            return new ContentContractsModel(parentObject, children);
        }).ToList();

        var allChildItems = allPossibleModels
            .SelectMany(model => model.Children)
            .ToHashSet();

        var finalItems = allPossibleModels
            .Where(model => !allChildItems.Contains(model.Parent));

        return finalItems.Concat(currencies);
    }

    public async Task<IEnumerable<KioskContentContractsModel>> FetchFederationContentForKioskContracts()
    {
        var federationContent = await FetchFederationContentLocal();
        var kiosks = federationContent
            .Where(item => item is KioskItem);

        var kioskModels = new List<KioskContentContractsModel>();
        foreach (var kiosk in kiosks)
        {
            if (kiosk is not KioskItem kioskItem)
                continue;
            var itemContract = await GetByContentId<ContractBase>(kioskItem.ItemType);
            var coinContract = await GetByContentId<ContractBase>(kioskItem.CurrencySymbol);
            var contentCoin = await _contentService.GetContent(kioskItem.CurrencySymbol);
            var contentItem = federationContent.First(x => x.Id.StartsWithFast(kioskItem.ItemType));
            kioskModels.Add(new KioskContentContractsModel(kioskItem, itemContract, coinContract, contentItem, contentCoin));
        }
        return kioskModels;
    }

    private async Task<List<IContentObject>> FetchFederationContentLocal()
    {
        return await GlobalCache.GetOrCreate<List<IContentObject>>(nameof(FetchFederationContentLocal), async _ =>
        {
            var federatedTypes = SuiFederationCommonHelper.GetFederationTypes();
            var manifest = await _contentService.GetManifest(new ContentQuery
            {
                TypeConstraints = federatedTypes
            });

            var result = new List<IContentObject>();
            foreach (var clientContentInfo in manifest.entries.Where(
                         item => item.contentId.StartsWith("currency") ||
                                 item.contentId.StartsWith("items") ||
                                 item.contentId.StartsWith(FederationContentTypes.KioskType)))
            {
                var contentRef = await clientContentInfo.Resolve();
                result.Add(contentRef);
            }
            return result;
        }, TimeSpan.FromDays(1)) ?? [];
    }

    public async Task<INftBase> GetItemContent(string contentId)
    {
        var contentRef = await _contentService.GetContent(contentId);
        return (contentRef as INftBase)!;
    }

    public async Task<Dictionary<string, INftBase>> GetItemContent(IEnumerable<string> contentIds)
    {
        var distinctIds = contentIds.Distinct();
        var tasks = distinctIds.Select(async id => new { Id = id, Content = await GetItemContent(id) });
        var results = await Task.WhenAll(tasks);
        return results.ToDictionary(x => x.Id, x => x.Content);
    }
}