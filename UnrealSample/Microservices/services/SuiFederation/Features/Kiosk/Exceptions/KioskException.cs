using System.Net;
using Beamable.Server;

namespace Beamable.SuiFederation.Features.Kiosk.Exceptions;

public class KioskException(string message)
    : MicroserviceException((int)HttpStatusCode.BadRequest, "KioskException", message);