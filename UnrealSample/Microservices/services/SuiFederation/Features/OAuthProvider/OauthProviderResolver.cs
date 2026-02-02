using Beamable.Common.Dependencies;
using Beamable.SuiFederation.Features.OAuthProvider.Exceptions;
using SuiFederationCommon.Models.Oauth;

namespace Beamable.SuiFederation.Features.OAuthProvider;

public class OauthProviderResolver : IService
{
    private readonly IDependencyProvider _dependencyProvider;

    public OauthProviderResolver(IDependencyProvider dependencyProvider)
    {
        _dependencyProvider = dependencyProvider;
    }

    public IOauthProvider Resolve(string providerName)
    {
        return providerName switch
        {
            OauthProvider.Google => _dependencyProvider.GetService<GoogleProvider>(),
            OauthProvider.Twitch => _dependencyProvider.GetService<TwitchProvider>(),
            _ => throw new OauthProviderException($"Provider ${providerName} doesn't have an implementation.")
        };
    }
}