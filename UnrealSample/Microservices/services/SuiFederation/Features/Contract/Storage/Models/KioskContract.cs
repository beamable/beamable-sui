using MongoDB.Bson.Serialization.Attributes;

namespace Beamable.SuiFederation.Features.Contract.Storage.Models;

[BsonDiscriminator(nameof(KioskContract))]
public class KioskContract : ContractBase
{
    public required string MarketPlaceOwnerCap { get; init; }
    public required string MarketPlace { get; init; }
    public required string ItemType { get; init; }
    public required string PriceContentId { get; init; }
}