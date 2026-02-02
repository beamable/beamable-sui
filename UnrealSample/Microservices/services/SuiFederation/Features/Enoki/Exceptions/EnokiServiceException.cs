using System.Net;
using Beamable.Server;

namespace Beamable.SuiFederation.Features.Enoki.Exceptions;

public class EnokiServiceException(string message)
    : MicroserviceException((int)HttpStatusCode.BadRequest, "EnokiServiceException", message);