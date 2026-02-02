using System;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Accounts.Exceptions;
using Beamable.SuiFederation.Features.Content.FunctionMessages;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Kiosk.Models;
using Beamable.SuiFederation.Features.Kiosk.Storage;
using Beamable.SuiFederation.Features.Kiosk.Storage.Models;
using Beamable.SuiFederation.Features.SuiApi;
using Beamable.SuiFederation.Features.SuiApi.Models;
using Beamable.SuiFederation.Features.Transactions;
using Beamable.SuiFederation.Features.Transactions.Storage.Models;

namespace Beamable.SuiFederation.Features.Kiosk.Handlers;

public class NftKioskHandler : IService, IKioskHandler
{
    private readonly AccountsService _accountsService;
    private readonly SuiApiService _suiApiService;
    private readonly TransactionManagerFactory _transactionManagerFactory;
    private readonly KioskListingCollection _kioskListingCollection;
    private readonly Configuration _configuration;

    public NftKioskHandler(AccountsService accountsService, SuiApiService suiApiService, TransactionManagerFactory transactionManagerFactory, KioskListingCollection kioskListingCollection, Configuration configuration)
    {
        _accountsService = accountsService;
        _suiApiService = suiApiService;
        _transactionManagerFactory = transactionManagerFactory;
        _kioskListingCollection = kioskListingCollection;
        _configuration = configuration;
    }

    public async Task ListForSale(KioskListModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var listMessage = new KioskListMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                "list",
                model.Wallet,
                model.KioskContract.MarketPlace,
                model.ItemProxyId,
                model.Price,
                account.PrivateKey);
            var result = await _suiApiService.ListForSale(listMessage);
            var listResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = listResult.digest,
                 Error = listResult.error,
                 Function = $"{nameof(NftKioskHandler)}.{nameof(ListForSale)}",
                 GasUsed = listResult.gasUsed,
                 Data = listMessage.SerializeSelected(),
                 Status = listResult.status,
             });
             if (listResult.status != "success")
             {
                 var message = $"{nameof(NftKioskHandler)}.{nameof(ListForSale)} failed with status {listResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(model.TransactionId, new Exception(message));
             }
             else
             {
                 await _kioskListingCollection.Insert(
                     KioskListing.Create(
                         listResult.createdId,
                         model.KioskContract.ContentId,
                         model.KioskItem.KioskName,
                         model.ItemContentId,
                         model.ItemProxyId,
                         model.ItemInventoryId,
                         model.KioskItem.CurrencySymbol,
                         model.Price,
                         model.GamerTag,
                         model.Wallet,
                         model.Namespace,
                         await _configuration.SuiEnvironment,
                         null
                         ));
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftKioskHandler)}.{nameof(ListForSale)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }

    public async Task DelistFromSale(KioskDelistModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var delistMessage = new KioskDelistMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                "delist",
                model.Wallet,
                model.KioskContract.MarketPlace,
                model.ListingId,
                account.PrivateKey);
            var result = await _suiApiService.DelistFromSale(delistMessage);
            var listResult = result is SuiTransactionResult transactionResult ? transactionResult : default;

            await transactionManager.AddChainTransaction(new ChainTransaction
            {
                Digest = listResult.digest,
                Error = listResult.error,
                Function = $"{nameof(NftKioskHandler)}.{nameof(DelistFromSale)}",
                GasUsed = listResult.gasUsed,
                Data = delistMessage.SerializeSelected(),
                Status = listResult.status,
            });
            if (listResult.status != "success")
            {
                var message = $"{nameof(NftKioskHandler)}.{nameof(ListForSale)} failed with status {listResult.status}";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(model.TransactionId, new Exception(message));
            }
            else
            {
                await _kioskListingCollection.Delete(model.ListingId);
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftKioskHandler)}.{nameof(DelistFromSale)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }

    public async Task PurchaseFromSale(KioskPurchaseModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var optionalTokenPolicy = model.CurrencyContract is GameCoinContract gc ? gc.TokenPolicy : string.Empty;
        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var purchaseMessage = new KioskPurchaseMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                "purchase",
                model.Wallet,
                model.KioskContract.MarketPlace,
                model.ListingId,
                model.CurrencyContract.PackageId,
                model.CurrencyContract.Module,
                model.Price,
                optionalTokenPolicy,
                account.PrivateKey);
            var result = await _suiApiService.KioskPurchase(purchaseMessage);
            var listResult = result is SuiTransactionResult transactionResult ? transactionResult : default;

            await transactionManager.AddChainTransaction(new ChainTransaction
            {
                Digest = listResult.digest,
                Error = listResult.error,
                Function = $"{nameof(NftKioskHandler)}.{nameof(PurchaseFromSale)}",
                GasUsed = listResult.gasUsed,
                Data = purchaseMessage.SerializeSelected(),
                Status = listResult.status,
            });
            if (listResult.status != "success")
            {
                var message = $"{nameof(NftKioskHandler)}.{nameof(PurchaseFromSale)} failed with status {listResult.status}";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(model.TransactionId, new Exception(message));
            }
            else
            {
                await _kioskListingCollection.Sold(model.ListingId, listResult.digest, account.Address);
            }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(NftKioskHandler)}.{nameof(PurchaseFromSale)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }
}