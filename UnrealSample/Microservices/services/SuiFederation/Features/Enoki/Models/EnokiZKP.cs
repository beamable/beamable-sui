using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Beamable.SuiFederation.Features.Enoki.Models;

public readonly record struct EnokiZkpRequest(
    string Network,
    string PublicKey,
    long MaxEpoch,
    string Randomness,
    string Token)
{
    public string ToCacheKey()
    {
        var bytes = Encoding.UTF8.GetBytes($"{Network}|{PublicKey}|{MaxEpoch}|{Randomness}|{Token}");
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}

public readonly record struct EnokiApiZkpRequest(
    [property: JsonPropertyName("network")] string SuiNetwork,
    [property: JsonPropertyName("ephemeralPublicKey")] string PublicKey,
    [property: JsonPropertyName("maxEpoch")] long MaxEpoch,
    [property: JsonPropertyName("randomness")] string Randomness);


public readonly record struct EnokiZkp(
    [property: JsonPropertyName("data")] ZkLoginProofData Data);

public readonly record struct ZkLoginProofData(
    [property: JsonPropertyName("proofPoints")] ProofPoints ProofPoints,
    [property: JsonPropertyName("issBase64Details")] IssBase64Details IssBase64Details,
    [property: JsonPropertyName("headerBase64")] string HeaderBase64,
    [property: JsonPropertyName("addressSeed")] string AddressSeed
);
public readonly record struct ProofPoints(
    [property: JsonPropertyName("a")] IReadOnlyList<string> A,
    [property: JsonPropertyName("b")] IReadOnlyList<IReadOnlyList<string>> B,
    [property: JsonPropertyName("c")] IReadOnlyList<string> C
);
public readonly record struct IssBase64Details(
    [property: JsonPropertyName("value")] string Value,
    [property: JsonPropertyName("indexMod4")] int IndexMod4
);