using Beamable.SuiFederation.Features.Contract.Storage.Models;
using SuiFederationCommon.FederationContent;

namespace Beamable.SuiFederation.Features.Kiosk.Models;

public record KioskListModel(long GamerTag, string Wallet, NftContract ItemContract, KioskContract KioskContract, KioskItem KioskItem, long ItemInventoryId, string ItemContentId, string ItemProxyId, long Price, string TransactionId, string Namespace);
public record KioskDelistModel(long GamerTag, string Wallet, string ListingId, KioskContract KioskContract, string TransactionId);
public record KioskPurchaseModel(long GamerTag, string Wallet, string ListingId, long Price, KioskContract KioskContract, ContractBase CurrencyContract, string TransactionId);