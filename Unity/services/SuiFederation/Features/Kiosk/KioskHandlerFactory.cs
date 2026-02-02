using System;
using Beamable.Common.Dependencies;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Kiosk.Handlers;
using Microsoft.Extensions.DependencyInjection;
using SuiFederationCommon;

namespace Beamable.SuiFederation.Features.Kiosk;

public class KioskHandlerFactory(
    IDependencyProvider serviceProvider) : IService
{
    public IKioskHandler GetHandler(MicroserviceInfo microserviceInfo)
    {
        return microserviceInfo.MicroserviceNamespace switch
        {
            SuiFederationSettings.SuiIdentityName => serviceProvider.GetRequiredService<NftKioskHandler>(),
            _ => throw new NotSupportedException($"Kiosk handler for '{microserviceInfo.MicroserviceNamespace}' is not supported.")
        };
    }
}