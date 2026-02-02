using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Beamable.SuiFederation.Features.Kiosk.Storage.Models;

public record KioskListing(
    [property: BsonId] string ListingId,
    string KioskContentId,
    string KioskName,
    string ItemContentId,
    string ItemProxyId,
    long ItemInventoryId,
    string PriceContentId,
    long Price,
    long GamerTag,
    string Wallet,
    string Network,
    string Namespace,
    DateTime CreatedAt,
    [property: BsonRepresentation(BsonType.String)] KioskListingStatus Status,
    string? SaleTxDigest,
    DateTime? SoldAt,
    string? BuyerWallet,
    KioskListingExclusiveBuyer? ExclusiveBuyer
)
{
    public static KioskListing Create(string listingId, string kioskContentId, string kioskName, string itemContentId, string itemProxyId, long itemInventoryId, string priceContentId, long price, long gamerTag, string wallet, string identityNamespace, string network,
        KioskListingExclusiveBuyer? exclusiveBuyer)
        => new(listingId, kioskContentId, kioskName, itemContentId, itemProxyId, itemInventoryId, priceContentId, price, gamerTag, wallet, network, identityNamespace, DateTime.UtcNow, KioskListingStatus.Active, null, null, null, exclusiveBuyer);
}

public record KioskListingExclusiveBuyer(
    long GamerTag,
    string Wallet,
    string PurchaseCap
);

public enum KioskListingStatus
{
    Active,
    Sold,
    Placed
}