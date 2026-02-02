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
    string? BuyerWallet
)
{
    public static KioskListing Create(string listingId, string kioskContentId, string kioskName, string itemContentId, string itemProxyId, long itemInventoryId, string priceContentId, long price, long gamerTag, string wallet, string identityNamespace, string network)
        => new(listingId, kioskContentId, kioskName, itemContentId, itemProxyId, itemInventoryId, priceContentId, price, gamerTag, wallet, network, identityNamespace, DateTime.UtcNow, KioskListingStatus.Active, null, null, null);
}

public enum KioskListingStatus
{
    Active,
    Sold
}