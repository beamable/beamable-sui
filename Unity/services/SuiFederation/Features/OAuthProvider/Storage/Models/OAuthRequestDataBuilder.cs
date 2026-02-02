using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Enoki.Models;
using Beamable.SuiFederation.Features.OAuthProvider.Models;

namespace Beamable.SuiFederation.Features.OAuthProvider.Storage.Models;

public class OAuthRequestDataBuilder
{
    private OAuthRequestData _data = null!;

    public OAuthRequestDataBuilder FromNewAuth(OAuthRequestData data, EnokiNonceDto enokiNonceDto, EnokiAddress enokiAddress)
    {
        _data = new OAuthRequestData
        {
            Id = data.Id,
            GamerTag = data.GamerTag,
            ClientId = data.ClientId,
            Provider = data.Provider,
            Namespace = data.Namespace,
            Network = data.Network,
            Nonce = enokiNonceDto.Data.Nonce,
            EphemeralPublicKey = enokiNonceDto.EphemeralPublicKey,
            EphemeralKey = enokiNonceDto.EphemeralKey,
            Randomness = enokiNonceDto.Data.Randomness,
            Url = data.Url,
            Epoch = enokiNonceDto.Data.Epoch,
            MaxEpoch = enokiNonceDto.Data.MaxEpoch,
            Token = data.Token,
            RefreshToken = data.RefreshToken,
            Address = new AddressData
            {
                Address = enokiAddress.Data.Address,
                PublicKey = enokiAddress.Data.PublicKey,
                Salt = enokiAddress.Data.Salt
            }
        };
        return this;
    }

    public OAuthRequestData Build()
    {
        return _data;
    }
}