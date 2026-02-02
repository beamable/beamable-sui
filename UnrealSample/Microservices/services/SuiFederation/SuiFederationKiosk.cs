using Beamable.Common;
using Beamable.Server;
using Beamable.SuiFederation.Endpoints.Kiosk;
using SuiFederationCommon.Models.Kiosk;

namespace Beamable.SuiFederation;

public partial class SuiFederation
{
    [ClientCallable]
    public async Promise<KioskListResponse> KioskList()
    {
        return await Provider.GetService<KioskListEndpoint>().KioskList();
    }

    [ClientCallable]
    public async Promise ListForSale(long itemId, long price, string optionalKioskContentId = "")
    {
        await Provider.GetService<KioskListEndpoint>().ListForSale(itemId, price, optionalKioskContentId);
    }

    [ClientCallable]
    public async Promise DelistFromSale(string listingId)
    {
        await Provider.GetService<KioskDelistEndpoint>().DelistFromSale(listingId);
    }

    [ClientCallable]
    public async Promise<KioskListingsResponse> OwnedListings(string optionalKioskContentId = "")
    {
        return await Provider.GetService<KioskListingsEndpoint>().OwnedListings(optionalKioskContentId);
    }

    [ClientCallable]
    public async Promise<KioskListingsResponsePaginated> AllListings(string optionalKioskContentId = "", int page = 1, int pageSize = 50)
    {
        return await Provider.GetService<KioskListingsEndpoint>().AllListings(optionalKioskContentId, page, pageSize);
    }

    [ClientCallable]
    public async Promise<KioskPurchaseResponse> KioskPurchase(string listingId)
    {
        return await Provider.GetService<KioskPurchaseEndpoint>().Purchase(listingId);
    }
}