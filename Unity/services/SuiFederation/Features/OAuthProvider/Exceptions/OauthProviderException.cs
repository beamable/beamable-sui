using System.Net;
using Beamable.Server;

namespace Beamable.SuiFederation.Features.OAuthProvider.Exceptions;

public class OauthProviderException(string message)
    : MicroserviceException((int)HttpStatusCode.BadRequest, "OauthProviderException", message);