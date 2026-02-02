using System.Text.Json.Serialization;
using Beamable.SuiFederation.Features.OAuthProvider.Storage.Models;

namespace Beamable.SuiFederation.Features.Enoki.Models;

public readonly record struct EnokiAddress(
    [property: JsonPropertyName("data")] EnokiAddressData Data);

public readonly record struct EnokiAddressData(
    [property: JsonPropertyName("salt")] string Salt,
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("publicKey")] string PublicKey);

public static class EnokiAddressExtensions
{
    public static AddressData ToAddressData(this EnokiAddressData address)
    {
        return new AddressData
        {
            Address = address.Address,
            Salt = address.Salt,
            PublicKey = address.PublicKey
        };
    }
}