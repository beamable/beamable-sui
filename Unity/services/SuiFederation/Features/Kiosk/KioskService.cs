using System;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Accounts.Exceptions;
using Beamable.SuiFederation.Features.Content.FunctionMessages;
using Beamable.SuiFederation.Features.Contract;
using Beamable.SuiFederation.Features.Contract.Handlers;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Kiosk.Models;
using Beamable.SuiFederation.Features.Kiosk.Storage;
using Beamable.SuiFederation.Features.Kiosk.Storage.Models;
using Beamable.SuiFederation.Features.SuiApi;
using Beamable.SuiFederation.Features.SuiApi.Models;
using Beamable.SuiFederation.Features.Transactions;
using Beamable.SuiFederation.Features.Transactions.Storage.Models;

namespace Beamable.SuiFederation.Features.Kiosk;

public class KioskService : IService
{
    private readonly SuiApiService _suiApiService;
    private readonly TransactionManagerFactory _transactionManagerFactory;
    private readonly PersonalKioskCollection _personalKioskCollection;
    private readonly AccountsService _accountsService;
    private readonly Configuration _configuration;
    private readonly KioskListingCollection _kioskListingCollection;
    private readonly PersonalKioskHandler _personalKioskHandler;
    private readonly RoyaltyKioskHandler _royaltyKioskHandler;
    public KioskService(SuiApiService suiApiService, TransactionManagerFactory transactionManagerFactory, PersonalKioskCollection personalKioskCollection, AccountsService accountsService, Configuration configuration, KioskListingCollection kioskListingCollection, PersonalKioskHandler personalKioskHandler, RoyaltyKioskHandler royaltyKioskHandler)
    {
        _suiApiService = suiApiService;
        _transactionManagerFactory = transactionManagerFactory;
        _personalKioskCollection = personalKioskCollection;
        _accountsService = accountsService;
        _configuration = configuration;
        _kioskListingCollection = kioskListingCollection;
        _personalKioskHandler = personalKioskHandler;
        _royaltyKioskHandler = royaltyKioskHandler;
    }

    private async ValueTask<BaseRulePackageIds?> GetBaseRulePackageIds()
    {
        if (await _configuration.SuiEnvironment == "devnet")
        {
            var personalKioskContract = await _personalKioskHandler.GetContract();
            var royaltyKioskContract = await _royaltyKioskHandler.GetContract();
            return new BaseRulePackageIds(royaltyKioskContract.PackageId, null, personalKioskContract.PackageId, null);
        }
        return null;
    }


    public async Task CreatePersonalKiosk(PersonalKioskCreateModel createModel)
    {
        var account = await _accountsService.GetAccount(createModel.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {createModel.GamerTag} not found.");
        var transactionManager = _transactionManagerFactory.Create(createModel.TransactionId);
        try
        {
            var createMessage = new PersonalKioskCreateMessage(
                createModel.KioskContract.ContentId,
                createModel.KioskContract.PackageId,
                createModel.KioskContract.Module,
                "create_personal_kiosk",
                createModel.Wallet,
                account.PrivateKey,
                await GetBaseRulePackageIds());
            var result = await _suiApiService.CreatePersonalKiosk(createMessage);
            var createResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = createResult.digest,
                 Error = createResult.error,
                 Function = $"{nameof(KioskService)}.{nameof(CreatePersonalKiosk)}",
                 GasUsed = createResult.gasUsed,
                 Data = createMessage.SerializeSelected(),
                 Status = createResult.status,
             });
             if (createResult.status != "success")
             {
                 var message = $"{nameof(KioskService)}.{nameof(CreatePersonalKiosk)} failed with status {createResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(createModel.TransactionId, new Exception(message));
             }
             else
             {
                 await _personalKioskCollection.Insert(new PersonalKiosk(createModel.GamerTag, createModel.Wallet, await _configuration.SuiEnvironment, createModel.Namespace, DateTime.UtcNow, createModel.KioskContract.ContentId, createResult.metadata?["kioskId"] ?? "", createResult.metadata?["personalKioskCap"] ?? ""));
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(KioskService)}.{nameof(CreatePersonalKiosk)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(createModel.TransactionId, new Exception(message));
        }
    }

    public async Task PersonalKioskList(PersonalKioskListModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var listMessage = new PersonalKioskListMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                string.IsNullOrWhiteSpace(model.ExclusiveBuyerWallet) ? "place_and_list" : "place_and_list_with_purchase_cap",
                model.Wallet,
                account.PrivateKey,
                model.PersonalKiosk.KioskId,
                model.PersonalKiosk.PersonalKioskCap,
                $"{model.ItemContract.PackageId}::{model.ItemContract.Module}::{model.ItemContract.Module.CapitalizeFirst()}",
                model.ItemProxyId,
                model.Price,
                model.ExclusiveBuyerWallet,
                await GetBaseRulePackageIds());
            var result = await _suiApiService.PersonalKioskList(listMessage);
            var listResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = listResult.digest,
                 Error = listResult.error,
                 Function = $"{nameof(KioskService)}.{nameof(PersonalKioskList)}",
                 GasUsed = listResult.gasUsed,
                 Data = listMessage.SerializeSelected(),
                 Status = listResult.status,
             });
             if (listResult.status != "success")
             {
                 var message = $"{nameof(KioskService)}.{nameof(PersonalKioskList)} failed with status {listResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(model.TransactionId, new Exception(message));
             }
             else
             {
                 KioskListingExclusiveBuyer? exclusiveBuyer = null;
                 if (!string.IsNullOrWhiteSpace(model.ExclusiveBuyerWallet))
                 {
                     exclusiveBuyer = new KioskListingExclusiveBuyer(model.ExclusiveBuyerId, model.ExclusiveBuyerWallet,
                         listResult.metadata!["kioskPurchaseCap"]);
                 }
                 await _kioskListingCollection.Insert(
                     KioskListing.Create(
                         listResult.createdId,
                         model.KioskContract.ContentId,
                         PlayerKioskHandler.ModuleName,
                         model.ItemContentId,
                         model.ItemProxyId,
                         model.ItemInventoryId,
                         PlayerKioskHandler.PriceSymbol,
                         model.Price,
                         model.GamerTag,
                         model.Wallet,
                         model.Namespace,
                         await _configuration.SuiEnvironment,
                         exclusiveBuyer
                     ));
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(KioskService)}.{nameof(PersonalKioskList)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }

    public async Task PersonalKioskDelist(PersonalKioskDelistModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var delistMessage = new PersonalKioskDelistMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                model.ReturnInventory ? "delist_and_take" : "delist",
                model.Wallet,
                account.PrivateKey,
                model.PersonalKiosk.KioskId,
                model.PersonalKiosk.PersonalKioskCap,
                $"{model.ItemContract.PackageId}::{model.ItemContract.Module}::{model.ItemContract.Module.CapitalizeFirst()}",
                model.ItemProxyId,
                await GetBaseRulePackageIds());
            var result = await _suiApiService.PersonalKioskDelist(delistMessage);
            var createResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = createResult.digest,
                 Error = createResult.error,
                 Function = $"{nameof(KioskService)}.{nameof(PersonalKioskDelist)}",
                 GasUsed = createResult.gasUsed,
                 Data = delistMessage.SerializeSelected(),
                 Status = createResult.status,
             });
             if (createResult.status != "success")
             {
                 var message = $"{nameof(KioskService)}.{nameof(PersonalKioskDelist)} failed with status {createResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(model.TransactionId, new Exception(message));
             }
             else
             {
                 await _kioskListingCollection.UpdateStatus(model.ItemProxyId, KioskListingStatus.Placed);
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(KioskService)}.{nameof(PersonalKioskDelist)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }

    public async Task PersonalKioskTake(PersonalKioskTakeModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var takeMessage = new PersonalKioskTakeMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                "take",
                model.Wallet,
                account.PrivateKey,
                model.PersonalKiosk.KioskId,
                model.PersonalKiosk.PersonalKioskCap,
                $"{model.ItemContract.PackageId}::{model.ItemContract.Module}::{model.ItemContract.Module.CapitalizeFirst()}",
                model.ItemProxyId,
                await GetBaseRulePackageIds());
            var result = await _suiApiService.PersonalKioskTake(takeMessage);
            var createResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = createResult.digest,
                 Error = createResult.error,
                 Function = $"{nameof(KioskService)}.{nameof(PersonalKioskTake)}",
                 GasUsed = createResult.gasUsed,
                 Data = takeMessage.SerializeSelected(),
                 Status = createResult.status,
             });
             if (createResult.status != "success")
             {
                 var message = $"{nameof(KioskService)}.{nameof(PersonalKioskTake)} failed with status {createResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(model.TransactionId, new Exception(message));
             }
             else
             {
                 await _kioskListingCollection.DeleteByProxy(model.ItemProxyId);
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(KioskService)}.{nameof(PersonalKioskTake)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }

    public async Task PersonalKioskDeclinePurchase(PersonalKioskDeclinePurchaseModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var declineMessage = new PersonalKioskDeclinePurchaseMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                "return_purchase_cap_to_seller",
                model.Wallet,
                account.PrivateKey,
                $"{model.ItemContract.PackageId}::{model.ItemContract.Module}::{model.ItemContract.Module.CapitalizeFirst()}",
                model.PurchaseCap,
                model.Wallet);
            var result = await _suiApiService.PersonalKioskDeclinePurchase(declineMessage);
            var listResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = listResult.digest,
                 Error = listResult.error,
                 Function = $"{nameof(KioskService)}.{nameof(PersonalKioskDeclinePurchase)}",
                 GasUsed = listResult.gasUsed,
                 Data = declineMessage.SerializeSelected(),
                 Status = listResult.status,
             });
             if (listResult.status != "success")
             {
                 var message = $"{nameof(KioskService)}.{nameof(PersonalKioskDeclinePurchase)} failed with status {listResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(model.TransactionId, new Exception(message));
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(KioskService)}.{nameof(PersonalKioskDeclinePurchase)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }

    public async Task PersonalKioskCancelExclusive(PersonalKioskCancelExclusiveModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var cancelExclusiveMessage = new PersonalKioskCancelExclusiveMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                "cancel_exclusive_listing",
                model.Wallet,
                account.PrivateKey,
                $"{model.ItemContract.PackageId}::{model.ItemContract.Module}::{model.ItemContract.Module.CapitalizeFirst()}",
                model.PurchaseCap,
                model.PersonalKiosk.KioskId,
                model.PersonalKiosk.PersonalKioskCap,
                await GetBaseRulePackageIds());
            var result = await _suiApiService.PersonalKioskCancelExclusive(cancelExclusiveMessage);
            var listResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = listResult.digest,
                 Error = listResult.error,
                 Function = $"{nameof(KioskService)}.{nameof(PersonalKioskCancelExclusive)}",
                 GasUsed = listResult.gasUsed,
                 Data = cancelExclusiveMessage.SerializeSelected(),
                 Status = listResult.status,
             });
             if (listResult.status != "success")
             {
                 var message = $"{nameof(KioskService)}.{nameof(PersonalKioskCancelExclusive)} failed with status {listResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(model.TransactionId, new Exception(message));
             }
             else
             {
                 await _kioskListingCollection.RemoveExclusive(model.ListingId);
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(KioskService)}.{nameof(PersonalKioskCancelExclusive)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }

    public async Task PersonalKioskPurchase(PersonalKioskPurchaseModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var purchaseMessage = new PersonalKioskPurchaseMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                string.IsNullOrWhiteSpace(model.PurchaseCap) ? "purchase" : "purchase_with_cap",
                model.Wallet,
                account.PrivateKey,
                model.BuyerPersonalKiosk.KioskId,
                model.SellerPersonalKiosk.KioskId,
                model.BuyerPersonalKiosk.PersonalKioskCap,
                $"{model.ItemContract.PackageId}::{model.ItemContract.Module}::{model.ItemContract.Module.CapitalizeFirst()}",
                model.ItemProxyId,
                model.ItemContract.Policy,
                model.Price,
                model.PurchaseCap,
                await GetBaseRulePackageIds());
            var result = await _suiApiService.PersonalKioskPurchase(purchaseMessage);
            var createResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = createResult.digest,
                 Error = createResult.error,
                 Function = $"{nameof(KioskService)}.{nameof(PersonalKioskPurchase)}",
                 GasUsed = createResult.gasUsed,
                 Data = purchaseMessage.SerializeSelected(),
                 Status = createResult.status,
             });
             if (createResult.status != "success")
             {
                 var message = $"{nameof(KioskService)}.{nameof(PersonalKioskPurchase)} failed with status {createResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(model.TransactionId, new Exception(message));
             }
             else
             {
                 await _kioskListingCollection.Sold(model.ListingId, createResult.digest, account.Address);
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(KioskService)}.{nameof(PersonalKioskPurchase)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }

    public async Task PersonalKioskWithdraw(PersonalKioskWithdrawModel model)
    {
        var account = await _accountsService.GetAccount(model.GamerTag.ToString());
        if (account is null)
            throw new UnknownAccountException($"Account for user id {model.GamerTag} not found.");

        var transactionManager = _transactionManagerFactory.Create(model.TransactionId);
        try
        {
            var withdrawMessage = new PersonalKioskWithdrawMessage(
                model.KioskContract.ContentId,
                model.KioskContract.PackageId,
                model.KioskContract.Module,
                "withdraw_profits",
                model.Wallet,
                account.PrivateKey,
                model.PersonalKiosk.KioskId,
                model.PersonalKiosk.PersonalKioskCap,
                model.Amount,
                await GetBaseRulePackageIds());
            var result = await _suiApiService.PersonalKioskWithdraw(withdrawMessage);
            var createResult = result is SuiTransactionResult transactionResult ? transactionResult : default;
             await transactionManager.AddChainTransaction(new ChainTransaction
             {
                 Digest = createResult.digest,
                 Error = createResult.error,
                 Function = $"{nameof(KioskService)}.{nameof(PersonalKioskWithdraw)}",
                 GasUsed = createResult.gasUsed,
                 Data = withdrawMessage.SerializeSelected(),
                 Status = createResult.status,
             });
             if (createResult.status != "success")
             {
                 var message = $"{nameof(KioskService)}.{nameof(PersonalKioskWithdraw)} failed with status {createResult.status}";
                 BeamableLogger.LogError(message);
                 await transactionManager.TransactionError(model.TransactionId, new Exception(message));
             }
        }
        catch (Exception e)
        {
            var message =
                $"{nameof(KioskService)}.{nameof(PersonalKioskWithdraw)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(model.TransactionId, new Exception(message));
        }
    }
}