using System.Threading.Tasks;
using Beamable.SuiFederation.Features.Enoki.Models;
using Beamable.SuiFederation.Features.OAuthProvider.Models;

namespace Beamable.SuiFederation.Features.OAuthProvider;

public interface IOauthProvider
{
    string Name { get; }
    Task<string> GetAuthorizationUrl(long gamerTag);
    Task<OAuthDto> Validate(OauthCallbackRequest callbackRequest);
    Task<EnokiAddress> ResolveAddress(long gamerTag, string challenge, string solution);
    Task<AuthCodeResponse> GetTokenFromCode(string code);
}