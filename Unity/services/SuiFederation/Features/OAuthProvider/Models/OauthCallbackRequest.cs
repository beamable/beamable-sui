using System.Text.Json.Serialization;

namespace Beamable.SuiFederation.Features.OAuthProvider.Models;

public readonly record struct OauthCallbackRequest(
    [property: JsonPropertyName("state")] string State,
    [property: JsonPropertyName("code")] string Code);