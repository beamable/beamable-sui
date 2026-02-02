using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Beamable.SuiFederation.Features.OAuthProvider.Models;
using Beamable.SuiFederation.Features.OAuthProvider.Storage.Models;

namespace Beamable.SuiFederation.Features.OAuthProvider;

public static class OauthProviderExtensions
{
    public static bool IsValid(this ClaimsPrincipal? claimsPrincipal, OAuthStatePayload statePayload)
    {
        if (claimsPrincipal is null)
            return false;

        if (claimsPrincipal.Identity?.IsAuthenticated != true)
            return false;

        var nonceClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "nonce");
        if (nonceClaim is null)
            return false;

        return nonceClaim.Value == statePayload.Nonce;
    }

    public static bool ValidateChallengeUri(this Uri challengeUri, OAuthRequestData requestData)
    {
        var queryParams = HttpUtility.ParseQueryString(challengeUri.Query);
        return queryParams["client_id"] == requestData.ClientId &&
               queryParams["nonce"] == requestData.Nonce;
    }
}