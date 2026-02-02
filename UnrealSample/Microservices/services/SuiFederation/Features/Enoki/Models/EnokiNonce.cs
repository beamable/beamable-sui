using System.Text.Json.Serialization;

namespace Beamable.SuiFederation.Features.Enoki.Models;

public readonly record struct EnokiNonce(
    [property: JsonPropertyName("data")] EnokiNonceData Data);

public readonly record struct EnokiNonceData(
    [property: JsonPropertyName("nonce")] string Nonce,
    [property: JsonPropertyName("randomness")] string Randomness,
    [property: JsonPropertyName("epoch")] long Epoch,
    [property: JsonPropertyName("maxEpoch")] long MaxEpoch,
    [property: JsonPropertyName("estimatedExpiration")] long EstimatedExpiration);

public readonly record struct EnokiNonceRequest(
    [property: JsonPropertyName("network")] string SuiNetwork,
    [property: JsonPropertyName("ephemeralPublicKey")] string PublicKey,
    [property: JsonPropertyName("additionalEpochs")] int AdditionalEpochs);

public readonly record struct EnokiNonceDto(
    EnokiNonceData Data,
    string EphemeralPublicKey,
    string EphemeralKey);