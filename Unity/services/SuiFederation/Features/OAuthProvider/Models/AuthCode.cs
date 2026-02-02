using System.Text.Json.Serialization;

namespace Beamable.SuiFederation.Features.OAuthProvider.Models;

public readonly record struct AuthCodeResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("id_token")] string Token,
    [property: JsonPropertyName("refresh_token")] string RefreshToken);