using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.Common.Content;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Content.FunctionMessages;
using Beamable.SuiFederation.Features.Content.Models;
using Beamable.SuiFederation.Features.Content.Storage;
using Beamable.SuiFederation.Features.Content.Storage.Models;
using Beamable.SuiFederation.Features.Contract;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Enoki;
using Beamable.SuiFederation.Features.Enoki.Models;
using Beamable.SuiFederation.Features.Inventory.Models;
using Beamable.SuiFederation.Features.Inventory.Storage;
using Beamable.SuiFederation.Features.OAuthProvider;
using Beamable.SuiFederation.Features.SuiApi;
using Beamable.SuiFederation.Features.SuiApi.Models;
using Beamable.SuiFederation.Features.Transactions;
using Beamable.SuiFederation.Features.Transactions.Storage.Models;
using SuiFederationCommon.Extensions;

namespace Beamable.SuiFederation.Features.Content.Handlers;

public class EnokiNftHandler : IService, IContentHandler
{
    private readonly NftHandler _innerHandler;
    private readonly ContractService _contractService;
    private readonly SuiApiService _suiApiService;
    private readonly TransactionManagerFactory _transactionManagerFactory;
    private readonly AccountsService _accountsService;
    private readonly OauthProviderService _oauthProviderService;
    private readonly EnokiService _enokiService;
    private readonly Configuration _configuration;
    private readonly NftAttachmentCollection _nftAttachmentCollection;

    public EnokiNftHandler(ContractService contractService, SuiApiService suiApiService, TransactionManagerFactory transactionManagerFactory, AccountsService accountsService, NftHandler innerHandler, OauthProviderService oauthProviderService, EnokiService enokiService, Configuration configuration, NftAttachmentCollection nftAttachmentCollection)
    {
        _contractService = contractService;
        _suiApiService = suiApiService;
        _transactionManagerFactory = transactionManagerFactory;
        _accountsService = accountsService;
        _innerHandler = innerHandler;
        _oauthProviderService = oauthProviderService;
        _enokiService = enokiService;
        _configuration = configuration;
        _nftAttachmentCollection = nftAttachmentCollection;
    }
    
    public Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequest inventoryRequest, IContentObject contentObject)
    {
        return _innerHandler.ConstructMessage(transaction, wallet, inventoryRequest, contentObject);
    }

    public async Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequestUpdate inventoryRequest,
        IContentObject contentObject)
    {
        var contract = await _contractService.GetByContentId<NftContract>(inventoryRequest.ToNftType());
        return new NftUpdateMessage(
            inventoryRequest.ContentId,
            contract.PackageId,
            contract.Module,
            "update",
            wallet,
            inventoryRequest.ProxyId,
            contract.OwnerInfo,
            "",
            NftContentItem.Empty,
            inventoryRequest.Properties
                .Select(kvp => new NftAttribute(kvp.Key, kvp.Value))
                .ToArray()
        );
    }

    public async Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequestDelete inventoryRequest,
        IContentObject contentObject)
    {
        var contract = await _contractService.GetByContentId<NftContract>(inventoryRequest.ToNftType());
        return new NftDeleteMessage(
            inventoryRequest.ContentId,
            contract.PackageId,
            contract.Module,
            "burn",
            wallet,
            inventoryRequest.ProxyId,
            contract.OwnerInfo,
            ""
        );
    }

    public async Task SendMessages(string transaction, string wallet, List<BaseMessage> messages)
    {
        if (messages.Count == 0) return;

        var lookup = messages.ToLookup(m => m.GetType());

        var mintMessages = lookup[typeof(NftMintMessage)].ToList();
        if (mintMessages.Count > 0)
        {
            await _innerHandler.SendMintMessages(transaction, mintMessages.Cast<NftMintMessage>().ToList());
        }

        var updateMessages = lookup[typeof(NftUpdateMessage)].ToList();
        if (updateMessages.Count > 0)
        {
            await SendUpdateMessages(transaction, wallet, updateMessages.Cast<NftUpdateMessage>().ToList());
        }

        var deleteMessages = lookup[typeof(NftDeleteMessage)].ToList();
        if (deleteMessages.Count > 0)
        {
            await SendDeleteMessages(transaction, wallet, deleteMessages.Cast<NftDeleteMessage>().ToList());
        }
    }

    private async Task SendDeleteMessages(string transaction, string wallet, List<NftDeleteMessage> messages)
    {
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            var gamerTag = await _accountsService.GetGamerTag(wallet);
            var oauthData = await _oauthProviderService.GetByGamer(gamerTag);

            if (oauthData is null)
                throw new Exception($"No oauth data found for gamertag {gamerTag} and transaction {transaction}");

            oauthData = await _oauthProviderService.TryRecreateAuthData(oauthData);
            var token = EncryptionService.Decrypt(oauthData.Token, _configuration.RealmSecret);

            var burnTransaction = await _suiApiService.DeleteNft(messages, true);
            var sponsoredTransactionCreate = await _enokiService.CreateSponsoredTransaction(new EnokiSponsoredTransactionCreateRequest
            {
                SuiNetwork = oauthData.Network,
                Jwt = token,
                TransactionBlockKindBytes = (burnTransaction is SuiEnokiTransactionResult result ? result : default).Id,
                PlayerWalletAddress = wallet,
                Functions = messages
                    .DistinctBy(msg => new { msg.PackageId, msg.Module, msg.Function })
                    .Select(msg => new EnokiSponsoredTransactionRequestFunction(
                        msg.PackageId,
                        msg.Module,
                        msg.Function
                    ))
                    .ToArray()
            });

            var zkp = await _enokiService.CreateZkp(new EnokiZkpRequest(oauthData.Network, oauthData.EphemeralPublicKey, oauthData.MaxEpoch, oauthData.Randomness, token));
            var sponsoredTransactionSignature = await _suiApiService.EnokiSignTransaction(zkp.Data, oauthData.MaxEpoch, sponsoredTransactionCreate.CreateData.Bytes, EncryptionService.Decrypt(oauthData.EphemeralKey, _configuration.RealmSecret));
            var sponsoredTransactionResult = await _enokiService
                .ExecuteSponsoredTransaction(new EnokiSponsoredTransactionExecuteRequest(sponsoredTransactionCreate.CreateData.Digest, sponsoredTransactionSignature));

            if (!sponsoredTransactionResult.IsEmpty())
            {
                await transactionManager.AddChainTransaction(new ChainTransaction
                {
                    Digest = sponsoredTransactionResult.CreateData.Digest,
                    Error = string.Empty,
                    Function = $"{nameof(EnokiGameCoinHandler)}.{nameof(SendDeleteMessages)}",
                    GasUsed = await _suiApiService.GetGasInfo(sponsoredTransactionResult.CreateData.Digest),
                    Data = messages.SerializeSelected(),
                    Status = "success",
                });
            }
            else
            {
                var message = $"{nameof(EnokiNftHandler)}.{nameof(SendDeleteMessages)} failed ";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(EnokiNftHandler)}.{nameof(SendDeleteMessages)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }

    private async Task SendUpdateMessages(string transaction, string wallet, List<NftUpdateMessage> messages)
    {
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            var gamerTag = await _accountsService.GetGamerTag(wallet);
            var oauthData = await _oauthProviderService.GetByGamer(gamerTag);

            if (oauthData is null)
                throw new Exception($"No oauth data found for gamertag {gamerTag} and transaction {transaction}");

            oauthData = await _oauthProviderService.TryRecreateAuthData(oauthData);
            var token = EncryptionService.Decrypt(oauthData.Token, _configuration.RealmSecret);

            var updateTransaction = await _suiApiService.UpdateNft(messages, true);
            var sponsoredTransactionCreate = await _enokiService.CreateSponsoredTransaction(new EnokiSponsoredTransactionCreateRequest
            {
                SuiNetwork = oauthData.Network,
                Jwt = token,
                TransactionBlockKindBytes = (updateTransaction is SuiEnokiTransactionResult result ? result : default).Id,
                PlayerWalletAddress = wallet,
                Functions = messages
                    .DistinctBy(msg => new { msg.PackageId, msg.Module, msg.Function })
                    .Select(msg => new EnokiSponsoredTransactionRequestFunction(
                        msg.PackageId,
                        msg.Module,
                        msg.Function
                    ))
                    .ToArray()
            });

            var zkp = await _enokiService.CreateZkp(new EnokiZkpRequest(oauthData.Network, oauthData.EphemeralPublicKey, oauthData.MaxEpoch, oauthData.Randomness, token));
            var sponsoredTransactionSignature = await _suiApiService.EnokiSignTransaction(zkp.Data, oauthData.MaxEpoch, sponsoredTransactionCreate.CreateData.Bytes, EncryptionService.Decrypt(oauthData.EphemeralKey, _configuration.RealmSecret));
            var sponsoredTransactionResult = await _enokiService
                .ExecuteSponsoredTransaction(new EnokiSponsoredTransactionExecuteRequest(sponsoredTransactionCreate.CreateData.Digest, sponsoredTransactionSignature));

            if (!sponsoredTransactionResult.IsEmpty())
            {
                await transactionManager.AddChainTransaction(new ChainTransaction
                {
                    Digest = sponsoredTransactionResult.CreateData.Digest,
                    Error = string.Empty,
                    Function = $"{nameof(EnokiNftHandler)}.{nameof(SendUpdateMessages)}",
                    GasUsed = await _suiApiService.GetGasInfo(sponsoredTransactionResult.CreateData.Digest),
                    Data = messages.SerializeSelected(),
                    Status = "success",
                });
            }
            else
            {
                var message = $"{nameof(EnokiNftHandler)}.{nameof(SendUpdateMessages)} failed ";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(EnokiNftHandler)}.{nameof(SendUpdateMessages)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }

    public async Task<IFederatedState> GetState(string wallet, string contentId)
    {
        var contentTypePart = contentId.ToContentType();
        var contract = await _contractService.GetByContentId<NftContract>(contentTypePart);
        var ownedObjects = (await _suiApiService.GetOwnedObjects(wallet, new GetOwnedObjectsRequest(contract.PackageId))).ToList();
        var possibleAttachments = (await _nftAttachmentCollection.GetByParentIds(ownedObjects.Select(o => o.ObjectId)))
            .DistinctBy(a => a.ParentProxy)
            .Select(a => a.ParentProxy);

        var attachments = new List<NftAttachmentResult>();
        foreach (var attachment in possibleAttachments)
        {
            var ownedAttachments = (await _suiApiService.GetAttachments(attachment)).ToList();
            if (ownedAttachments.Count > 0)
            {
                ownedObjects.AddRange(ownedAttachments);
                attachments.Add(new NftAttachmentResult
                {
                    ParentProxy = attachment,
                    ChildProxy = ownedAttachments.Select(o => o.ObjectId).ToArray()
                });
            }
        }
        return new ItemsState
        {
            Items = ownedObjects.ToInventoryState(attachments)
        };
    }
}