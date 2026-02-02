using Beamable.Common;
using Beamable.Server;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Accounts.Exceptions;
using Beamable.SuiFederation.Features.OAuthProvider;
using SuiFederationCommon;
using SuiFederationCommon.Models.Oauth;

namespace Beamable.SuiFederation.Endpoints;

public class AuthenticateEnokiEndpoint : IEndpoint
{
    private readonly Configuration _configuration;
    private readonly RequestContext _requestContext;
    private readonly OauthProviderResolver _oauthProviderResolver;

    public AuthenticateEnokiEndpoint(Configuration configuration, RequestContext requestContext, OauthProviderResolver oauthProviderResolver)
    {
        _configuration = configuration;
        _requestContext = requestContext;
        _oauthProviderResolver = oauthProviderResolver;
    }

    public async Promise<FederatedAuthenticationResponse> Authenticate(string token, string challenge, string solution)
    {
        // Check if an external identity is already attached (from request context)
        if (_requestContext.UserId != 0L)
        {
            var microserviceInfo = MicroserviceMetadataExtensions.GetMetadata<SuiFederation, SuiWeb3EnokiIdentity>();
            var existingExternalIdentity = _requestContext.GetExternalIdentity(microserviceInfo);
            if (existingExternalIdentity is not null)
            {
                return new FederatedAuthenticationResponse
                {
                    user_id = existingExternalIdentity.userId
                };
            }
        }

        if (!OauthProvider.TryParse(token, out var oauthProvider))
        {
            throw new UnauthorizedException("Unknown Oauth provider");
        }
        var provider = _oauthProviderResolver.Resolve(oauthProvider);

        // Validate the challenge and solution
        if (!string.IsNullOrWhiteSpace(challenge) && !string.IsNullOrWhiteSpace(solution))
        {
            var addressData = await provider.ResolveAddress(_requestContext.UserId, challenge, solution);
            if (addressData.IsEmpty())
            {
                throw new UnauthorizedException("Invalid challenge or solution");
            }
            return new FederatedAuthenticationResponse
            {
                user_id = addressData.Data.Address
            };
        }

        var authorizationUrl = await provider.GetAuthorizationUrl(_requestContext.UserId);

        return new FederatedAuthenticationResponse
        {
            challenge = authorizationUrl,
            challenge_ttl = await _configuration.AuthenticationChallengeTtlSec
        };
    }
}