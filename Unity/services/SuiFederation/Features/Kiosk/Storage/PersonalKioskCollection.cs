using System.Threading.Tasks;
using Beamable.Server;
using Beamable.SuiFederation.Features.Kiosk.Storage.Models;
using MongoDB.Driver;

namespace Beamable.SuiFederation.Features.Kiosk.Storage;

public class PersonalKioskCollection(IStorageObjectConnectionProvider storageObjectConnectionProvider)
    : IService
{
    private IMongoCollection<PersonalKiosk>? _collection;

    private async ValueTask<IMongoCollection<PersonalKiosk>> Get()
    {
        if (_collection is not null) return _collection;
        _collection =
            (await storageObjectConnectionProvider.SuiFederationStorageDatabase()).GetCollection<PersonalKiosk>("kiosk-personal");

        return _collection;
    }

    public async Task<bool> Insert(PersonalKiosk listing)
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

    public async Task<PersonalKiosk?> GetByPlayer(long gamerTag)
    {
        var collection = await Get();
        var filter = Builders<PersonalKiosk>.Filter.Eq(x => x.GamerTag, gamerTag);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }
}