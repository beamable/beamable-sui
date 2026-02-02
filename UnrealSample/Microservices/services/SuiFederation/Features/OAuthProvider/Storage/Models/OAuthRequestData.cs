using System;
using Beamable.SuiFederation.Features.OAuthProvider.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SuiFederationCommon.Models.Notifications;

namespace Beamable.SuiFederation.Features.OAuthProvider.Storage.Models;

public class OAuthRequestData
{
    [BsonId]
    public ObjectId Id { get; set; }
    public long GamerTag { get; init; }
    public required string ClientId { get; init; }
    public required string Provider { get; init; }
    public required string Namespace { get; init; }
    public required string Network { get; init; }
    public required string Nonce { get; init; }
    public required string EphemeralPublicKey { get; init; }
    public required string EphemeralKey { get; init; }
    public required string Randomness { get; init; }
    public required string Url { get; init; }
    public long Epoch { get; init; }
    public long MaxEpoch { get; init; }
    public required string RefreshToken { get; init; }
    public required string Token { get; init; }
    public AddressData Address { get; init; } =  new();
    public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
}

public class AddressData
{
    public string Address { get; init; } = "";
    public string Salt { get; set; } = "";
    public string PublicKey { get; set; } = "";
}

public static class OAuthRequestDataExtensions
{
    public static OAuthDto ToDto(this OAuthRequestData data)
    {
        return new OAuthDto
        {
            Id = data.Id.ToString(),
            GamerTag = data.GamerTag,
            ClientId = data.ClientId,
            Network = data.Network,
            Provider = data.Provider,
            Namespace = data.Namespace,
            Nonce = data.Nonce,
            EphemeralPublicKey = data?.EphemeralPublicKey ?? "",
            EphemeralKey = data?.EphemeralKey ?? "",
            Randomness = data?.Randomness ?? "",
            Url = data?.Url ?? "",
            Epoch = data?.Epoch ?? 0,
            MaxEpoch = data?.MaxEpoch ?? 0,
            RefreshToken = data?.RefreshToken ?? "",
            Token = data?.Token ?? "",
            Address = data?.Address ?? new AddressData(),
            State = OAuthState.Authorized
        };
    }

    public static OAuthDto Failed(OAuthRequestData? data = null)
    {
        return new OAuthDto
        {
            Id = data?.Id.ToString() ?? "",
            GamerTag = data?.GamerTag ?? 0,
            ClientId = data?.ClientId ?? "",
            Network = data?.Network ?? "",
            Provider = data?.Provider ?? "",
            Namespace = data?.Namespace ?? "",
            Nonce = data?.Nonce ?? "",
            EphemeralPublicKey = data?.EphemeralPublicKey ?? "",
            EphemeralKey = data?.EphemeralKey ?? "",
            Randomness = data?.Randomness ?? "",
            Url = data?.Url ?? "",
            Epoch = data?.GamerTag ?? 0,
            MaxEpoch = data?.MaxEpoch ?? 0,
            RefreshToken = data?.RefreshToken ?? "",
            Token = data?.Token ?? "",
            Address = data?.Address ?? new AddressData(),
            State = OAuthState.Denied
        };
    }
}