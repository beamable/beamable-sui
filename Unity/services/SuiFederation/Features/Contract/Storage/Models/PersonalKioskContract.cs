using MongoDB.Bson.Serialization.Attributes;

namespace Beamable.SuiFederation.Features.Contract.Storage.Models;

[BsonDiscriminator(nameof(PersonalKioskContract))]
public class PersonalKioskContract : ContractBase
{
}