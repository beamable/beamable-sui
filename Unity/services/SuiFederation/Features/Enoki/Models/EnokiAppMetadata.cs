using System.Collections.Immutable;
using System.Linq;
using System.Text.Json.Serialization;

namespace Beamable.SuiFederation.Features.Enoki.Models;

public readonly record struct EnokiAppMetadata(
    [property: JsonPropertyName("data")] AppMetadata Data);

public readonly record struct AppMetadata(
    [property: JsonPropertyName("authenticationProviders")] ImmutableList<AuthenticationProvider> AuthenticationProviders);

public readonly record struct AuthenticationProvider(
    [property: JsonPropertyName("providerType")] string Provider,
    [property: JsonPropertyName("clientId")] string ClientId);


public static class EnokiAppMetadataExtensions
{
    public static string GetProviderClientId(this EnokiAppMetadata metadata, string provider)
    {
        foreach (var authProvider in metadata.Data.AuthenticationProviders.Where(authProvider => authProvider.Provider == provider))
        {
            return authProvider.ClientId;
        }
        return string.Empty;
    }
}