using System;
using System.Text.Json;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.SuiFederation.Features.Notifications;
using Beamable.SuiFederation.Features.OAuthProvider.Models;
using SuiFederationCommon.Models.Notifications;
using SuiFederationCommon.Models.Oauth;

namespace Beamable.SuiFederation.Features.OAuthProvider;

public class OAuthProviderCallbackService : IService
{
    private readonly PlayerNotificationService _playerNotificationService;
    private readonly OauthProviderResolver _oauthProviderResolver;

    public OAuthProviderCallbackService(PlayerNotificationService playerNotificationService, OauthProviderResolver oauthProviderResolver)
    {
        _playerNotificationService = playerNotificationService;
        _oauthProviderResolver = oauthProviderResolver;
    }


    public async Task<OauthCallbackResponse> ProcessCallback(string body)
    {
        try
        {
            var callbackModel = JsonSerializer.Deserialize<OauthCallbackRequest>(body);
            var state = CreateOAuthStatePayloadExtensions.ParseState(callbackModel.State);

            var oauthProvider = _oauthProviderResolver.Resolve(state.Provider);
            var oauthRequest = await oauthProvider.Validate(callbackModel);

            await _playerNotificationService.SendPlayerNotification(state.GamerTag, new OAuthNotification
            {
                Id = oauthRequest.Id,
                State = (oauthRequest.State != OAuthState.Authorized) ? OAuthState.Denied : OAuthState.Authorized
            });

            return OauthCallbackResponse.OkResult("Please go back to the game to attach your Enoki Wallet.");
        }
        catch (Exception e)
        {
            BeamableLogger.LogWarning("Error processing OAuth callback: {Error}", e.Message);
            return OauthCallbackResponse.FailedResult();
        }
    }
}