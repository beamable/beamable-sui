using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.Common.Content;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Content.FunctionMessages;
using Beamable.SuiFederation.Features.Content.Handlers.Extensions;
using Beamable.SuiFederation.Features.Content.Models;
using Beamable.SuiFederation.Features.Content.Storage;
using Beamable.SuiFederation.Features.Content.Storage.Models;
using Beamable.SuiFederation.Features.Contract;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Inventory.Models;
using Beamable.SuiFederation.Features.Inventory.Storage;
using Beamable.SuiFederation.Features.Inventory.Storage.Models;
using Beamable.SuiFederation.Features.SuiApi;
using Beamable.SuiFederation.Features.SuiApi.Models;
using Beamable.SuiFederation.Features.Transactions;
using Beamable.SuiFederation.Features.Transactions.Storage.Models;
using SuiFederationCommon.Extensions;
using SuiFederationCommon.Models;

namespace Beamable.SuiFederation.Features.Content.Handlers;

public class NftHandler : IService, IContentHandler
{
    private readonly ContractService _contractService;
    private readonly SuiApiService _suiApiService;
    private readonly TransactionManagerFactory _transactionManagerFactory;
    private readonly MintCollection _mintCollection;
    private readonly AccountsService _accountsService;
    private readonly NftAttachmentCollection _nftAttachmentCollection;

    public NftHandler(ContractService contractService, SuiApiService suiApiService, TransactionManagerFactory transactionManagerFactory, MintCollection mintCollection, AccountsService accountsService, NftAttachmentCollection nftAttachmentCollection)
    {
        _contractService = contractService;
        _suiApiService = suiApiService;
        _transactionManagerFactory = transactionManagerFactory;
        _mintCollection = mintCollection;
        _accountsService = accountsService;
        _nftAttachmentCollection = nftAttachmentCollection;
    }

    public async Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequest inventoryRequest, IContentObject contentObject)
    {
        var contract = await _contractService.GetByContentId<NftContract>(inventoryRequest.ToNftType());
        var nftItem = NftContentItemExtensions.Create(inventoryRequest, (contentObject as INftBase)!);
        return new NftMintMessage(
            inventoryRequest.ContentId,
            contract.PackageId,
            contract.Module,
            "mint",
            wallet,
            contract.AdminCap,
            nftItem);
    }

    public async Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequestUpdate inventoryRequest,
        IContentObject contentObject)
    {
        var playerAccount = await _accountsService.GetAccountByAddress(wallet);

        var (finalAttachmentId, functionName) = inventoryRequest.ResolveUpdateActions();

        var attachmentNft = string.IsNullOrWhiteSpace(finalAttachmentId) ? NftContentItem.Empty :
            NftContentItemExtensions.Create(finalAttachmentId, await _contractService.GetItemContent(finalAttachmentId));

        // If we have an attachment, we need to get the contract for that attachment
        var contract = string.IsNullOrWhiteSpace(finalAttachmentId) ? await _contractService.GetByContentId<NftContract>(inventoryRequest.ToNftType()) :
            await _contractService.GetByContentId<NftContract>(finalAttachmentId.ToContentType());

        //Are we updating the attachment
        var possibleAttachment = (await _nftAttachmentCollection.FindByAttachmentObject([inventoryRequest.ProxyId])).FirstOrDefault();
        if (possibleAttachment is not null)
        {
            return new NftUpdateMessage(
                inventoryRequest.ContentId,
                contract.PackageId,
                contract.Module,
                FunctionMessageNames.NftAttachmentUpdate,
                wallet,
                possibleAttachment.ParentProxy,
                contract.OwnerInfo,
                playerAccount!.PrivateKey,
                NftContentItemExtensions.CreateForUpdate(inventoryRequest.ContentId, await _contractService.GetItemContent(inventoryRequest.ContentId), inventoryRequest.Properties.Select(p => new NftAttribute(p.Key, p.Value)).ToList()),
                inventoryRequest.Properties
                    .Where(kvp => !ExcludedUpdateProperties().Contains(kvp.Key))
                    .Select(kvp => new NftAttribute(kvp.Key, kvp.Value))
                    .ToArray()
            );
        }

        return new NftUpdateMessage(
            inventoryRequest.ContentId,
            contract.PackageId,
            contract.Module,
            functionName,
            wallet,
            inventoryRequest.ProxyId,
            contract.OwnerInfo,
            playerAccount!.PrivateKey,
            attachmentNft,
            inventoryRequest.Properties
                .Where(kvp => !ExcludedUpdateProperties().Contains(kvp.Key))
                .Select(kvp => new NftAttribute(kvp.Key, kvp.Value))
                .ToArray()
        );
    }

    public async Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequestDelete inventoryRequest,
        IContentObject contentObject)
    {
        var contract = await _contractService.GetByContentId<NftContract>(inventoryRequest.ToNftType());
        var playerAccount = await _accountsService.GetAccountByAddress(wallet);
        return new NftDeleteMessage(
            inventoryRequest.ContentId,
            contract.PackageId,
            contract.Module,
            "burn",
            wallet,
            inventoryRequest.ProxyId,
            contract.OwnerInfo,
            playerAccount!.PrivateKey
        );
    }

    public async Task SendMessages(string transaction, string wallet, List<BaseMessage> messages)
{
    if (messages.Count == 0) return;

    var groupedMessages = messages.GroupBy(m => m.GetType());

    foreach (var group in groupedMessages)
    {
        switch (group.Key.Name)
        {
            case nameof(NftMintMessage):
                await SendMintMessages(transaction, group.Cast<NftMintMessage>().ToList());
                break;

            case nameof(NftDeleteMessage):
                await SendDeleteMessages(transaction, group.Cast<NftDeleteMessage>().ToList());
                break;

            case nameof(NftUpdateMessage):
                var updatesByFunction = group.Cast<NftUpdateMessage>().ToLookup(m => m.Function);

                if (updatesByFunction[FunctionMessageNames.NftUpdate].Any())
                    await SendUpdateMessages(transaction, updatesByFunction[FunctionMessageNames.NftUpdate].ToList());

                if (updatesByFunction[FunctionMessageNames.NftAttach].Any())
                    await SendAttachMessages(transaction, updatesByFunction[FunctionMessageNames.NftAttach].ToList());

                if (!updatesByFunction[FunctionMessageNames.NftDetach].Any())
                    await SendDetachMessages(transaction, updatesByFunction[FunctionMessageNames.NftDetach].ToList());

                if (updatesByFunction[FunctionMessageNames.NftAttachmentUpdate].Any())
                    await SendAttachmentUpdateMessages(transaction, updatesByFunction[FunctionMessageNames.NftAttachmentUpdate].ToList());
                break;
        }
    }
}

    private async Task SendDeleteMessages(string transaction, List<NftDeleteMessage> messages)
    {
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            // Handle attachments delete
            var possibleAttachments = await _nftAttachmentCollection.FindByAttachmentObject(messages.Select(m => m.ProxyId));
            var attachmentObjectIds = new HashSet<string>(
                possibleAttachments.SelectMany(att => att.AttachmentObjects)
            );
            var filteredDeleteMessages = messages
                .Where(dm => !attachmentObjectIds.Contains(dm.ProxyId))
                .ToList();
            var detachDeleteMessages = messages
                .Where(dm => attachmentObjectIds.Contains(dm.ProxyId))
                .ToList();
            if (detachDeleteMessages.Count > 0)
            {
                var attachmentToParent = possibleAttachments
                    .SelectMany(att => att.AttachmentObjects.Select(obj => new { Key = obj, Value = att.ParentProxy }))
                    .GroupBy(x => x.Key)
                    .ToDictionary(g => g.Key, g => g.First().Value);

                await SendDetachMessages(transaction, detachDeleteMessages.Select(x =>
                    new NftUpdateMessage(
                        x.ContentId,
                        x.PackageId,
                        x.Module,
                        FunctionMessageNames.NftDetach,
                        x.PlayerWalletAddress,
                        attachmentToParent[x.ProxyId],
                        x.OwnerObjectId,
                        x.PlayerWalletKey,
                        new NftContentItem("", "", "", x.ContentId, ImmutableArray<NftAttribute>.Empty),
                        [])).ToList());
            }

            if (filteredDeleteMessages.Count == 0) return;
            var result = await _suiApiService.DeleteNft(filteredDeleteMessages);
            var deleteResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
            await transactionManager.AddChainTransaction(new ChainTransaction
            {
                Digest = deleteResult.digest,
                Error = deleteResult.error,
                Function = $"{nameof(NftHandler)}.{nameof(SendDeleteMessages)}",
                GasUsed = deleteResult.gasUsed,
                Data = filteredDeleteMessages.SerializeSelected(),
                Status = deleteResult.status,
            });
            if (deleteResult.status != "success")
            {
                var message = $"{nameof(NftHandler)}.{nameof(SendDeleteMessages)} failed with status {deleteResult.status}";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftHandler)}.{nameof(SendDeleteMessages)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }

    private async Task SendUpdateMessages(string transaction, List<NftUpdateMessage> messages)
    {
        if (messages.Count == 0) return;
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            var result = await _suiApiService.UpdateNft(messages);
            var updateResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
            await transactionManager.AddChainTransaction(new ChainTransaction
            {
                Digest = updateResult.digest,
                Error = updateResult.error,
                Function = $"{nameof(NftHandler)}.{nameof(SendUpdateMessages)}",
                GasUsed = updateResult.gasUsed,
                Data = messages.SerializeSelected(),
                Status = updateResult.status,
            });
            if (updateResult.status != "success")
            {
                var message = $"{nameof(NftHandler)}.{nameof(SendUpdateMessages)} failed with status {updateResult.status}";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftHandler)}.{nameof(SendUpdateMessages)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }

    private async Task SendAttachMessages(string transaction, List<NftUpdateMessage> messages)
    {
        if (messages.Count == 0) return;
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            var result = await _suiApiService.AttachNft(messages);
            var updateResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
            await transactionManager.AddChainTransaction(new ChainTransaction
            {
                Digest = updateResult.digest,
                Error = updateResult.error,
                Function = $"{nameof(NftHandler)}.{nameof(SendAttachMessages)}",
                GasUsed = updateResult.gasUsed,
                Data = messages.SerializeSelected(),
                Status = updateResult.status,
            });
            if (updateResult.status != "success")
            {
                var message = $"{nameof(NftHandler)}.{nameof(SendAttachMessages)} failed with status {updateResult.status}";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }
            else
            {
                await _mintCollection.InsertMints(
                    messages.Select(m => new Mint
                    {
                        PackageId = m.PackageId,
                        Module = m.Module,
                        Digest = updateResult.digest,
                        ContentId = m.Attachment.ContentId,
                        InitialOwnerAddress = m.PlayerWalletAddress,
                        Metadata = m.ToAttachmentMetadata(),
                        ObjectIds = updateResult.objectIds
                    }));

                await _nftAttachmentCollection.Insert(
                    messages.Select(message =>
                        NftAttachment.Create(message.ProxyId, message.ContentId, message.Attachment.ContentId, message.Attachment.Name, updateResult.objectIds)));

            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftHandler)}.{nameof(SendAttachMessages)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }

    private async Task SendDetachMessages(string transaction, List<NftUpdateMessage> messages)
    {
        if (messages.Count == 0) return;
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            var result = await _suiApiService.DetachNft(messages);
            var updateResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
            await transactionManager.AddChainTransaction(new ChainTransaction
            {
                Digest = updateResult.digest,
                Error = updateResult.error,
                Function = $"{nameof(NftHandler)}.{nameof(SendDetachMessages)}",
                GasUsed = updateResult.gasUsed,
                Data = messages.SerializeSelected(),
                Status = updateResult.status,
            });
            if (updateResult.status != "success")
            {
                var message = $"{nameof(NftHandler)}.{nameof(SendDetachMessages)} failed with status {updateResult.status}";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }
            else
            {
                foreach (var message in messages)
                {
                    await _nftAttachmentCollection.DeleteByParentIds(message.ProxyId, message.Attachment.ContentId);
                }
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftHandler)}.{nameof(SendDetachMessages)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }

    private async Task SendAttachmentUpdateMessages(string transaction, List<NftUpdateMessage> messages)
    {
        if (messages.Count == 0) return;
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            var result = await _suiApiService.NftAttachmentUpdate(messages);
            var updateResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
            await transactionManager.AddChainTransaction(new ChainTransaction
            {
                Digest = updateResult.digest,
                Error = updateResult.error,
                Function = $"{nameof(NftHandler)}.{nameof(SendAttachmentUpdateMessages)}",
                GasUsed = updateResult.gasUsed,
                Data = messages.SerializeSelected(),
                Status = updateResult.status,
            });
            if (updateResult.status != "success")
            {
                var message = $"{nameof(NftHandler)}.{nameof(SendAttachmentUpdateMessages)} failed with status {updateResult.status}";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftHandler)}.{nameof(SendAttachmentUpdateMessages)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }

    public async Task SendMintMessages(string transaction, List<NftMintMessage> messages)
    {
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            var result = await _suiApiService.MintNfts(messages);
            await transactionManager.AddChainTransaction(new ChainTransaction
            {
                Digest = result.digest,
                Error = result.error,
                Function = $"{nameof(NftHandler)}.{nameof(SendMintMessages)}",
                GasUsed = result.gasUsed,
                Data = messages.SerializeSelected(),
                Status = result.status,
            });
            if (result.status != "success")
            {
                var message = $"{nameof(NftHandler)}.{nameof(SendMintMessages)} failed with status {result.status}";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }
            else
            {
                await _mintCollection.InsertMints(
                    messages.Select(m => new Mint
                    {
                        PackageId = m.PackageId,
                        Module = m.Module,
                        Digest = result.digest,
                        ContentId = m.ContentId,
                        InitialOwnerAddress = m.PlayerWalletAddress,
                        Metadata = m.ToMetadata(),
                        ObjectIds = result.objectIds
                    }));
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftHandler)}.{nameof(SendMintMessages)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }

    public async Task<IFederatedState> GetState(string wallet, string contentId)
    {
        var contentTypePart = contentId.ToContentType();
        var contract = await _contractService.GetByContentId<NftContract>(contentTypePart);
        var ownedObjects = (await _suiApiService.GetOwnedObjects(wallet, new GetOwnedObjectsRequest(contract.PackageId))).ToList();

        BeamableLogger.Log("NFT GetState Count: {count}", ownedObjects.Count);

        var possibleParents = (await _nftAttachmentCollection.GetByParentIds(ownedObjects.Select(o => o.ObjectId)))
            .DistinctBy(a => a.ParentProxy)
            .Select(a => a.ParentProxy);

        var attachments = new List<NftAttachmentResult>();
        foreach (var attachment in possibleParents)
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

    private static HashSet<string> ExcludedUpdateProperties()
        => ["$attach","$detach"];
}