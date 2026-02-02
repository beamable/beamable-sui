using MongoDB.Bson.Serialization.Attributes;

namespace Beamable.SuiFederation.Features.Contract.Storage.Models;

[BsonDiscriminator(nameof(RoyaltyKioskContract))]
public class RoyaltyKioskContract : ContractBase
{
}