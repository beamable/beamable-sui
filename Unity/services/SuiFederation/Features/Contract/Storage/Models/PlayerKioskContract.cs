using MongoDB.Bson.Serialization.Attributes;

namespace Beamable.SuiFederation.Features.Contract.Storage.Models;

[BsonDiscriminator(nameof(PlayerKioskContract))]
public class PlayerKioskContract : ContractBase
{

}