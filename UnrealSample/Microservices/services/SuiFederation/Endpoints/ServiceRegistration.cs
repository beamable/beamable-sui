using Beamable.Common.Dependencies;
using Beamable.SuiFederation.Endpoints.Kiosk;

namespace Beamable.SuiFederation.Endpoints;

public static class ServiceRegistration
{
    public static void AddEndpoints(this IDependencyBuilder builder)
    {
        builder.AddScoped<AuthenticateEndpoint>();
        builder.AddScoped<StartInventoryTransactionEndpoint>();
        builder.AddScoped<GetInventoryStateEndpoint>();
        builder.AddScoped<AuthenticateExternalEndpoint>();
        builder.AddScoped<WithdrawalEndpoint>();
        builder.AddScoped<AuthenticateEnokiEndpoint>();
        builder.AddScoped<GetFederationInfoEndpoint>();
        builder.AddScoped<KioskListEndpoint>();
        builder.AddScoped<KioskDelistEndpoint>();
        builder.AddScoped<KioskListingsEndpoint>();
        builder.AddScoped<KioskPurchaseEndpoint>();
    }
}