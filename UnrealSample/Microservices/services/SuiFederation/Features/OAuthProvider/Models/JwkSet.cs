using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Beamable.SuiFederation.Features.OAuthProvider.Models;

public readonly record struct JwkSet(
    [property: JsonPropertyName("keys")] ImmutableList<JwkKey> Data);

public readonly record struct JwkKey(
    [property: JsonPropertyName("alg")] string Algorithm,
    [property: JsonPropertyName("e")] string Exponent,
    [property: JsonPropertyName("kty")] string KeyType,
    [property: JsonPropertyName("n")] string Modulus,
    [property: JsonPropertyName("use")] string Use,
    [property: JsonPropertyName("kid")] string KeyId);