using System.Linq;
using System.Threading.Tasks;
using Beamable.Server;
using Beamable.Server.Content;
using Beamable.SuiFederation.Features.Content.Models;
using Beamable.SuiFederation.Features.Contract;
using Beamable.SuiFederation.Features.Kiosk.Storage;
using SuiFederationCommon.Models.Kiosk;

namespace Beamable.SuiFederation.Endpoints.Kiosk;

public class KioskListingsEndpoint : IEndpoint
{
    private readonly RequestContext _requestContext;
    private readonly KioskListingCollection _kioskListingCollection;
    private readonly ContractService _contractService;
    public KioskListingsEndpoint(RequestContext requestContext, KioskListingCollection kioskListingCollection, ContractService contractService)
    {
        _requestContext = requestContext;
        _kioskListingCollection = kioskListingCollection;
        _contractService = contractService;
    }

    public async Task<KioskListingsResponse> OwnedListings(string optionalKioskContentId)
    {
        var listings = await _kioskListingCollection.OwnedListings(_requestContext.UserId, optionalKioskContentId);
        var contentItems = await _contractService.GetItemContent(listings.Select(l => l.ItemContentId));

        return new KioskListingsResponse
        {
            listings = listings.Select(l => new KioskListing
            {
                listingId = l.ListingId,
                kioskContentId = l.KioskContentId,
                kioskName = l.KioskName,
                itemContentId = l.ItemContentId,
                itemProxyId = l.ItemProxyId,
                itemInventoryId = l.ItemInventoryId,
                priceContentId = l.PriceContentId,
                price = l.Price,
                gamerTag = l.GamerTag,
                wallet = l.Wallet,
                createdAt = l.CreatedAt,
                status = l.Status.ToString(),
                properties = contentItems[l.ItemContentId].ToItemProperties()
            }).ToList()
        };
    }

    public async Task<KioskListingsResponsePaginated> AllListings(string optionalKioskContentId, int page, int pageSize)
    {
        var listings = await _kioskListingCollection.AllListings(optionalKioskContentId, page, pageSize);
        var contentItems = await _contractService.GetItemContent(listings.Items.Select(l => l.ItemContentId));
        return new KioskListingsResponsePaginated
        {
            page = page,
            pageSize = pageSize,
            totalCount = listings.TotalCount,
            listings = listings.Items.Select(l => new KioskListing
            {
                listingId = l.ListingId,
                kioskContentId = l.KioskContentId,
                kioskName = l.KioskName,
                itemContentId = l.ItemContentId,
                itemProxyId = l.ItemProxyId,
                itemInventoryId = l.ItemInventoryId,
                priceContentId = l.PriceContentId,
                price = l.Price,
                gamerTag = l.GamerTag,
                wallet = l.Wallet,
                createdAt = l.CreatedAt,
                status = l.Status.ToString(),
                properties = contentItems[l.ItemContentId].ToItemProperties()
            }).ToList()
        };
    }
}