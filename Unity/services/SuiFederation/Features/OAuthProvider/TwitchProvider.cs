using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.Common.Api;
using Beamable.SuiFederation.Caching;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Enoki;
using Beamable.SuiFederation.Features.Enoki.Models;
using Beamable.SuiFederation.Features.HttpService;
using Beamable.SuiFederation.Features.HttpService.Models;
using Beamable.SuiFederation.Features.OAuthProvider.Exceptions;
using Beamable.SuiFederation.Features.OAuthProvider.Models;
using Beamable.SuiFederation.Features.OAuthProvider.Storage;
using Beamable.SuiFederation.Features.OAuthProvider.Storage.Models;
using Microsoft.IdentityModel.Tokens;
using SuiFederationCommon;
using SuiFederationCommon.Models.Oauth;
using Swan;

namespace Beamable.SuiFederation.Features.OAuthProvider;

public class TwitchProvider : IService, IOauthProvider
{
    private readonly EnokiService _enokiService;
    private readonly OAuthRequestCollection _oAuthRequestCollection;
    private readonly Configuration _configuration;
    private readonly HttpClientService _httpClientService;
    private readonly IBeamableRequester _beamableRequester;

    public TwitchProvider(EnokiService enokiService, OAuthRequestCollection oAuthRequestCollection, Configuration configuration, HttpClientService httpClientService, IBeamableRequester beamableRequester)
    {
        _enokiService = enokiService;
        _oAuthRequestCollection = oAuthRequestCollection;
        _configuration = configuration;
        _httpClientService = httpClientService;
        _beamableRequester = beamableRequester;
    }

    public string Name => OauthProvider.Twitch;

    public async Task<string> GetAuthorizationUrl(long gamerTag)
    {
        var metadata = await _enokiService.GetAppMetadata();
        var clientId = metadata.GetProviderClientId(Name);
        var nonceData = await _enokiService.CreateNonce();
        var state = CreateOAuthStatePayloadExtensions.GenerateStateParameter(gamerTag,Name, nonceData.Data.Nonce);
        var redirectUri = $"https://api.beamable.com/basic/{_beamableRequester.Cid}.{_beamableRequester.Pid}.micro_{SuiFederationSettings.MicroserviceName}/OAuthCallback";
        var url = $"https://id.twitch.tv/oauth2/authorize?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope=openid&nonce={nonceData.Data.Nonce}&state={state}";
        await _oAuthRequestCollection.Insert(new OAuthRequestData
        {
            GamerTag = gamerTag,
            ClientId = clientId,
            Provider = Name,
            Namespace = SuiFederationSettings.SuiEnokiIdentityName,
            Network = await _configuration.SuiEnvironment,
            Nonce = nonceData.Data.Nonce,
            EphemeralPublicKey = nonceData.EphemeralPublicKey,
            EphemeralKey = EncryptionService.Encrypt(nonceData.EphemeralKey, _configuration.RealmSecret),
            Randomness = nonceData.Data.Randomness,
            Epoch = nonceData.Data.Epoch,
            MaxEpoch = nonceData.Data.MaxEpoch,
            Url = url,
            RefreshToken = string.Empty,
            Token = string.Empty
        });
        return url;
    }

    public async Task<AuthCodeResponse> GetTokenFromCode(string code)
    {
        try
        {
            var metadata = await _enokiService.GetAppMetadata();
            var clientId = metadata.GetProviderClientId(Name);
            return await _httpClientService.Post<AuthCodeResponse>("https://id.twitch.tv/oauth2/token", new UrlRequestBody
            {
                {"client_id", clientId},
                {"client_secret", await _configuration.TwitchClientSecret},
                {"code", code},
                {"grant_type", "authorization_code"},
                {"redirect_uri", $"https://api.beamable.com/basic/{_beamableRequester.Cid}.{_beamableRequester.Pid}.micro_{SuiFederationSettings.MicroserviceName}/OAuthCallback" },
            });
        }
        catch (Exception e)
        {
            BeamableLogger.LogWarning("Failed to fetch token for code with error {error}", e.Message);
            return default;
        }
    }

    public async Task<OAuthDto> Validate(OauthCallbackRequest callbackRequest)
    {
        try
        {
            var state = CreateOAuthStatePayloadExtensions.ParseState(callbackRequest.State);
            var oAuthRequest = await _oAuthRequestCollection.GetByNonce(state.Nonce);
            if (oAuthRequest is null)
            {
                BeamableLogger.LogWarning("OAuth request not found for the provided nonce {0} and user {1}", state.Nonce, state.GamerTag);
                return OAuthRequestDataExtensions.Failed(oAuthRequest);
            }

            var metadata = await _enokiService.GetAppMetadata();
            var clientId = metadata.GetProviderClientId(Name);

            var authResponse = await GetTokenFromCode(callbackRequest.Code);

            var jwkSet = await GetJwkSet();
            var keys = new JsonWebKeySet(JsonSerializer.Serialize(jwkSet)).Keys;
            var validationParams = new TokenValidationParameters
            {
                ValidIssuer = "https://id.twitch.tv/oauth2",
                ValidAudience = clientId,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = keys,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();
            var claimsPrincipal = handler.ValidateToken(authResponse.Token, validationParams, out _);
            if (!claimsPrincipal.IsValid(state))
            {
                BeamableLogger.LogWarning("Invalid token received for gamer {GamerTag}. Claims principal is not valid.", oAuthRequest.GamerTag);
                return OAuthRequestDataExtensions.Failed(oAuthRequest);
            }
            await _oAuthRequestCollection.UpdateAuth(oAuthRequest.Id, EncryptionService.Encrypt(authResponse.Token, _configuration.RealmSecret),
                EncryptionService.Encrypt(authResponse.RefreshToken, _configuration.RealmSecret));
            return oAuthRequest.ToDto();
        }
        catch (Exception e)
        {
            BeamableLogger.LogWarning("Failed to validate {0} OAuth callback: {1}", Name, e.Message);
            return OAuthRequestDataExtensions.Failed();
        }
    }

    public async Task<EnokiAddress> ResolveAddress(long gamerTag, string challenge, string solution)
    {
        try
        {
            var requestData = await _oAuthRequestCollection.GetById(solution);
            if (requestData is null)
            {
                BeamableLogger.LogWarning("OAuth request not found for solution", solution);
                return default;
            }

            var challengeUri = new Uri(challenge);
            if (!challengeUri.ValidateChallengeUri(requestData) || gamerTag != requestData.GamerTag)
            {
                BeamableLogger.LogWarning("Failed to validate challenge for gamerTag: {gamerTag} with solution: {solution}", gamerTag, solution);
                return default;
            }

            var addressData = await _enokiService.GetAddress(EncryptionService.Decrypt(requestData.Token, _configuration.RealmSecret));
            if (addressData.IsEmpty())
            {
                BeamableLogger.LogWarning("Failed to resolve address for gamerTag: {gamerTag} with solution: {solution}", gamerTag, solution);
                return default;
            }
            await _oAuthRequestCollection.UpdateAddress(requestData.Id, addressData.Data);
            return addressData;
        }
        catch (Exception e)
        {
            BeamableLogger.LogWarning("Failed to resolve {0} OAuth: {1}", Name, e.Message);
            return default;
        }
    }

    private async Task<JwkSet> GetJwkSet()
    {
        return await GlobalCache.GetOrCreate<JwkSet>($"{Name}-jwkCache", async _ =>
        {
            try
            {
                var data = await _httpClientService.Get<JwkSet>("https://id.twitch.tv/oauth2/keys");
                return data;
            }
            catch (Exception)
            {
                BeamableLogger.LogWarning("Failed to get JWK Set from {0}", Name);
                return null;
            }
        }, TimeSpan.FromHours(1)) ?? default;
    }
}