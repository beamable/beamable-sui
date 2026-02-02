using Beamable.SuiFederation.Features.OAuthProvider.Storage.Models;
using SuiFederationCommon.Models.Notifications;

namespace Beamable.SuiFederation.Features.OAuthProvider.Models;

public record OAuthDto
{
    public required string Id { get; init; } = "";
    public required long GamerTag { get; init; }
    public required string ClientId { get; init; } = "";
    public required string Namespace { get; init; } = "";
    public required string Provider { get; init; } = "";
    public required string Network { get; init; } = "";
    public required string EphemeralPublicKey { get; init; } = "";
    public required string EphemeralKey { get; init; } = "";
    public required string Nonce { get; init; } = "";
    public required string Randomness { get; init; } = "";
    public required string Url { get; init; } = "";
    public required long Epoch { get; init; }
    public required long MaxEpoch { get; init; }
    public required string RefreshToken { get; init; } = "";
    public required string Token { get; init; } = "";
    public required AddressData Address { get; init; } = new();
    public required OAuthState State { get; init; }
}