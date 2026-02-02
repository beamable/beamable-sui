using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beamable.Server;
using Beamable.SuiFederation.Features.Common;
using Beamable.SuiFederation.Features.Kiosk.Exceptions;
using Beamable.SuiFederation.Features.Kiosk.Storage.Models;
using MongoDB.Driver;

namespace Beamable.SuiFederation.Features.Kiosk.Storage;

public class KioskListingCollection(IStorageObjectConnectionProvider storageObjectConnectionProvider)
    : IService
{
    private IMongoCollection<KioskListing>? _collection;

    private async ValueTask<IMongoCollection<KioskListing>> Get()
    {
        if (_collection is not null) return _collection;
        _collection =
            (await storageObjectConnectionProvider.SuiFederationStorageDatabase()).GetCollection<KioskListing>("kiosk-listings");
        await _collection.Indexes.CreateManyAsync([
            new CreateIndexModel<KioskListing>(Builders<KioskListing>.IndexKeys
                .Ascending(x => x.KioskContentId)),
            new CreateIndexModel<KioskListing>(Builders<KioskListing>.IndexKeys
                .Ascending(x => x.KioskContentId)
                .Ascending(x => x.GamerTag)),
            new CreateIndexModel<KioskListing>(Builders<KioskListing>.IndexKeys
                .Ascending(x => x.GamerTag)),
            new CreateIndexModel<KioskListing>(Builders<KioskListing>.IndexKeys
                .Ascending(x => x.ItemInventoryId)),
            new CreateIndexModel<KioskListing>(Builders<KioskListing>.IndexKeys
                .Ascending(x => x.ListingId))
        ]);
        return _collection;
    }

    public async Task<bool> Insert(KioskListing listing)
    {
        var collection = await Get();
        try
        {
            await collection.InsertOneAsync(listing);
            return true;
        }
        catch (MongoWriteException)
        {
            return false;
        }
    }

    public async Task Delete(string listingId)
    {
        var collection = await Get();
        try
        {
            var filter = Builders<KioskListing>.Filter.Eq(x => x.ListingId, listingId);
            await collection.DeleteOneAsync(filter);
        }
        catch (MongoWriteException)
        {
        }
    }

    public async Task DeleteByProxy(string itemProxyId)
    {
        var collection = await Get();
        try
        {
            var filter = Builders<KioskListing>.Filter.Eq(x => x.ItemProxyId, itemProxyId);
            await collection.DeleteOneAsync(filter);
        }
        catch (MongoWriteException)
        {
        }
    }

    public async Task Delete(string kioskContentId, string network)
    {
        var collection = await Get();
        try
        {
            var filter = Builders<KioskListing>.Filter.Eq(x => x.KioskContentId, kioskContentId) &
                         Builders<KioskListing>.Filter.Eq(x => x.Network, network);
            await collection.DeleteOneAsync(filter);
        }
        catch (MongoWriteException)
        {
        }
    }

    public async Task<KioskListing> GetByItemInventoryId(long itemInventoryId)
    {
        var collection = await Get();
        var filter = Builders<KioskListing>.Filter.Eq(x => x.ItemInventoryId, itemInventoryId);
        var listing = await collection.Find(filter).FirstOrDefaultAsync();
        if (listing is null)
            throw new KioskException($"Item with inventory ID {itemInventoryId} is not listed on the kiosk.");
        return listing;
    }

    public async Task<KioskListing> GetByListingId(string listingId)
    {
        var collection = await Get();
        var filter = Builders<KioskListing>.Filter.Eq(x => x.ListingId, listingId);
        var listing = await collection.Find(filter).FirstOrDefaultAsync();
        if (listing is null)
            throw new KioskException($"Item with listing ID {listingId} is not listed on the kiosk.");
        return listing;
    }

    public async Task UpdateStatus(string itemProxyId, KioskListingStatus kioskListingStatus)
    {
        var collection = await Get();
        var filter = Builders<KioskListing>.Filter.Eq(x => x.ItemProxyId, itemProxyId);
        var update = Builders<KioskListing>.Update
            .Set(x => x.Status, kioskListingStatus);
        await collection.UpdateOneAsync(filter, update);
    }

    public async Task RemoveExclusive(string listingId)
    {
        var collection = await Get();
        var filter = Builders<KioskListing>.Filter.Eq(x => x.ListingId, listingId);
        await collection.DeleteOneAsync(filter);
    }

    public async Task Sold(string listingId, string txDigest, string buyerWallet)
    {
        var collection = await Get();
        var filter = Builders<KioskListing>.Filter.Eq(x => x.ListingId, listingId);
        var update = Builders<KioskListing>.Update
            .Set(x => x.Status, KioskListingStatus.Sold)
            .Set(x => x.SoldAt, DateTime.UtcNow)
            .Set(x => x.SaleTxDigest, txDigest)
            .Set(x => x.BuyerWallet, buyerWallet);
        await collection.UpdateOneAsync(filter, update);
    }

    public async Task<List<KioskListing>> OwnedListings(long gamerTag, string optionalKioskContentId)
    {
        var collection = await Get();
        var filter = string.IsNullOrWhiteSpace(optionalKioskContentId) ? Builders<KioskListing>.Filter.Eq(x => x.GamerTag, gamerTag)
            : Builders<KioskListing>.Filter.Eq(x => x.KioskContentId, optionalKioskContentId) & Builders<KioskListing>.Filter.Eq(x => x.GamerTag, gamerTag);
        return await collection.Find(filter).ToListAsync();
    }

    public async Task<PaginatedResult<KioskListing>> AllListings(string optionalKioskContentId, int page = 1, int pageSize = 50)
    {
        var collection = await Get();
        var filter = string.IsNullOrWhiteSpace(optionalKioskContentId) ? Builders<KioskListing>.Filter.Empty
            : Builders<KioskListing>.Filter.Eq(x => x.KioskContentId, optionalKioskContentId);
        var totalCount = string.IsNullOrWhiteSpace(optionalKioskContentId) ? await collection.EstimatedDocumentCountAsync() :
            await collection.CountDocumentsAsync(filter);
        var items = await collection.Find(filter)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PaginatedResult<KioskListing>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }
}