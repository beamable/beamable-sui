using System.Threading.Tasks;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Enoki;
using Beamable.SuiFederation.Features.OAuthProvider.Storage;
using Beamable.SuiFederation.Features.OAuthProvider.Storage.Models;
using Beamable.SuiFederation.Features.SuiApi;
using SuiFederationCommon;

namespace Beamable.SuiFederation.Features.OAuthProvider;

public class OauthProviderService : IService
{
    private readonly OAuthRequestCollection _oAuthRequestCollection;
    private readonly Configuration _configuration;
    private readonly EnokiService _enokiService;
    private readonly SuiApiService _suiApiService;

    public OauthProviderService(OAuthRequestCollection oAuthRequestCollection, Configuration configuration, EnokiService enokiService, SuiApiService suiApiService)
    {
        _oAuthRequestCollection = oAuthRequestCollection;
        _configuration = configuration;
        _enokiService = enokiService;
        _suiApiService = suiApiService;
    }

    public async Task<OAuthRequestData?> GetByGamer(long gamerTag, string nameSpace = SuiFederationSettings.SuiEnokiIdentityName)
    {
        var network = await _configuration.SuiEnvironment;
        return await _oAuthRequestCollection.GetByGamer(gamerTag, network, nameSpace);
    }

    public async Task<OAuthRequestData> TryRecreateAuthData(OAuthRequestData data)
    {
        var currentEpoch = await _suiApiService.GetCurrentEpoch();
        if (currentEpoch < data.MaxEpoch)
            return data;

        var nonceData = await _enokiService.CreateNonce();
        var addressData = await _enokiService.GetAddress(EncryptionService.Decrypt(data.Token, _configuration.RealmSecret));
        var builder = new OAuthRequestDataBuilder();
        builder.FromNewAuth(data, nonceData, addressData);
        var newData = builder.Build();
        await _oAuthRequestCollection.Insert(newData);
        return newData;
    }
}