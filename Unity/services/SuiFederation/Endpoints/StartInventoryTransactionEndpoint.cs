using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Beamable.Common;
using Beamable.Common.Api.Inventory;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.ChannelProcessor;
using Beamable.SuiFederation.Features.Content.Models;
using Beamable.SuiFederation.Features.Inventory;
using Beamable.SuiFederation.Features.Inventory.Models;
using Beamable.SuiFederation.Features.Transactions;
using SuiFederationCommon.Models.Notifications;

namespace Beamable.SuiFederation.Endpoints;

public class StartInventoryTransactionEndpoint : IEndpoint
{
    private readonly TransactionManager _transactionManager;
    private readonly InventoryService _inventoryService;
    private readonly UpdatePlayerStateService _updatePlayerStateService;
    private readonly GetInventoryStateEndpoint _getInventoryStateEndpoint;

    public StartInventoryTransactionEndpoint(TransactionManager transactionManager, InventoryService inventoryService, UpdatePlayerStateService updatePlayerStateService, GetInventoryStateEndpoint getInventoryStateEndpoint)
    {
        _transactionManager = transactionManager;
        _inventoryService = inventoryService;
        _updatePlayerStateService = updatePlayerStateService;
        _getInventoryStateEndpoint = getInventoryStateEndpoint;
    }

    public async Promise<FederatedInventoryProxyState> StartInventoryTransaction(string id, string transaction, Dictionary<string, long> currencies, List<FederatedItemCreateRequest> newItems, List<FederatedItemDeleteRequest> deleteItems, List<FederatedItemUpdateRequest> updateItems, long gamerTag, MicroserviceInfo microserviceInfo)
    {
        var transactionId = await _transactionManager.StartTransaction(id, nameof(StartInventoryTransaction), transaction, currencies, newItems, deleteItems, updateItems);
        _transactionManager.SetCurrentTransactionContext(transactionId);
        _ = _transactionManager.RunAsyncBlock(transactionId, transaction, async () =>
        {
            // NEW ITEMS
            var currencyRequest = currencies.Select(c => new InventoryRequest(gamerTag, c.Key, c.Value, ImmutableDictionary<string, string>.Empty));
            var itemsRequest = newItems.Select(i => new InventoryRequest(gamerTag, i.contentId, 1, i.properties.ToImmutableDictionary()));
            await ChannelService.Enqueue(gamerTag, async (_) =>
                await _inventoryService.NewItems(transactionId.ToString(), id, currencyRequest.Union(itemsRequest), gamerTag)
            );

            // UPDATE ITEMS
            var updateItemsRequest = updateItems.Select(i => new InventoryRequestUpdate(gamerTag,
                i.contentId, i.proxyId,
                i.properties
                    .Where(kvp => !NftContentItemExtensions.FixedProperties().Contains(kvp.Key))
                    .ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value)));
            await ChannelService.Enqueue(gamerTag, async (_) =>
                await _inventoryService.UpdateItems(transactionId.ToString(), id, updateItemsRequest, gamerTag)
            );

            // DELETE ITEMS
            var deleteItemsRequest = deleteItems.Select(i => new InventoryRequestDelete(gamerTag,
                i.contentId, i.proxyId));
            await ChannelService.Enqueue(gamerTag, async (_) =>
                await _inventoryService.DeleteItems(transactionId.ToString(), id, deleteItemsRequest, gamerTag)
            );

            // UPDATE PLAYER STATE
            await ChannelService.Enqueue(gamerTag, async (_) =>
                    await _updatePlayerStateService.Update(new InventoryTransactionNotification
                    {
                        Id = transaction
                    }, _getInventoryStateEndpoint, microserviceInfo)
            );
            BeamableLogger.Log("ChannelService queue length {GetQueueLength}", ChannelService.GetQueueLength());
        });

        return await _inventoryService.GetLastKnownState(id);
    }
}