using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Kiosk.Storage.Models;

namespace Beamable.SuiFederation.Features.Kiosk.Models;

public record PersonalKioskCreateModel(long GamerTag, string Wallet, PlayerKioskContract KioskContract, string TransactionId, string Namespace);
public record PersonalKioskListModel(long GamerTag, string Wallet, string ItemContentId, long ItemInventoryId, NftContract ItemContract, PersonalKiosk PersonalKiosk, PlayerKioskContract KioskContract, string ItemProxyId, long Price, string TransactionId, string Namespace, long ExclusiveBuyerId, string ExclusiveBuyerWallet);
public record PersonalKioskDelistModel(long GamerTag, string Wallet, NftContract ItemContract, PersonalKiosk PersonalKiosk, PlayerKioskContract KioskContract, string ItemProxyId, string TransactionId, string Namespace, bool ReturnInventory);
public record PersonalKioskTakeModel(long GamerTag, string Wallet, NftContract ItemContract, PersonalKiosk PersonalKiosk, PlayerKioskContract KioskContract, string ItemProxyId, string TransactionId, string Namespace);
public record PersonalKioskDeclinePurchaseModel(long GamerTag, string ListingId, string Wallet, NftContract ItemContract, PlayerKioskContract KioskContract, string Seller, string PurchaseCap, string TransactionId, string Namespace);
public record PersonalKioskCancelExclusiveModel(long GamerTag, string ListingId, string Wallet, PersonalKiosk PersonalKiosk, NftContract ItemContract, PlayerKioskContract KioskContract, string Seller, string PurchaseCap, string TransactionId, string Namespace);
public record PersonalKioskPurchaseModel(long GamerTag, string Wallet, NftContract ItemContract, PersonalKiosk BuyerPersonalKiosk, PersonalKiosk SellerPersonalKiosk, PlayerKioskContract KioskContract, string ItemProxyId, long Price, string TransactionId, string Namespace, string ListingId, string PurchaseCap);
public record PersonalKioskWithdrawModel(long GamerTag, string Wallet, PersonalKiosk PersonalKiosk, PlayerKioskContract KioskContract, long Amount, string TransactionId, string Namespace);