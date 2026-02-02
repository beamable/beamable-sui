using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Beamable.SuiFederation.Features.OAuthProvider.Exceptions;

namespace Beamable.SuiFederation.Features.OAuthProvider.Models;

public readonly record struct OAuthStatePayload(
    [property: JsonPropertyName("provider")] string Provider = "",
    [property: JsonPropertyName("nonce")] string Nonce = "",
    [property: JsonPropertyName("gamerTag")] long GamerTag = 0);

public static class CreateOAuthStatePayloadExtensions
{
    public static string GenerateStateParameter(long gamerTag, string provider, string nonce)
    {
        var payload = new OAuthStatePayload(provider, nonce, gamerTag);
        var json = JsonSerializer.Serialize(payload);
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        return Uri.EscapeDataString(base64);
    }

    public static OAuthStatePayload ParseState(string base64State)
    {
        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64State));
            return JsonSerializer.Deserialize<OAuthStatePayload>(json);
        }
        catch (Exception)
        {
            return new OAuthStatePayload();
        }
    }
}