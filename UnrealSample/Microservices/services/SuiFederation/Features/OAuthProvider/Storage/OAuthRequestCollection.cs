using System;
using System.Linq;
using System.Threading.Tasks;
using Beamable.Server;
using Beamable.SuiFederation.Features.Enoki.Models;
using Beamable.SuiFederation.Features.OAuthProvider.Exceptions;
using Beamable.SuiFederation.Features.OAuthProvider.Storage.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Beamable.SuiFederation.Features.OAuthProvider.Storage;

public class OAuthRequestCollection(IStorageObjectConnectionProvider storageObjectConnectionProvider, Configuration configuration)
    : IService
{
    private IMongoCollection<OAuthRequestData>? _collection;

    private async ValueTask<IMongoCollection<OAuthRequestData>> Get()
    {
        if (_collection is not null) return _collection;
        _collection =
            (await storageObjectConnectionProvider.SuiFederationStorageDatabase()).GetCollection<OAuthRequestData>("oauth-requests");
        await _collection.Indexes.CreateManyAsync([
            new CreateIndexModel<OAuthRequestData>(Builders<OAuthRequestData>.IndexKeys
                .Ascending(x => x.Nonce)
                .Ascending(x => x.Network)),
            new CreateIndexModel<OAuthRequestData>(Builders<OAuthRequestData>.IndexKeys
                .Ascending(x => x.GamerTag)
                .Ascending(x => x.Network)
                .Ascending(x => x.Provider)),
            new CreateIndexModel<OAuthRequestData>(Builders<OAuthRequestData>.IndexKeys
                .Ascending(x => x.GamerTag)
                .Ascending(x => x.Network)
                .Ascending(x => x.Namespace))
        ]);
        return _collection;
    }

    public async Task Insert(OAuthRequestData model)
    {
        try
        {
            var collection = await Get();
            var filter = Builders<OAuthRequestData>.Filter.And(
                Builders<OAuthRequestData>.Filter.Eq(m => m.GamerTag, model.GamerTag),
            Builders<OAuthRequestData>.Filter.Eq(m => m.Network, model.Network),
                Builders<OAuthRequestData>.Filter.Eq(m => m.Provider, model.Provider)
            );

            var update = Builders<OAuthRequestData>.Update
                .SetOnInsert(x => x.Id, ObjectId.GenerateNewId())
                .Set(x => x.GamerTag, model.GamerTag)
                .Set(x => x.ClientId, model.ClientId)
                .Set(x => x.Namespace, model.Namespace)
                .Set(x => x.Provider, model.Provider)
                .Set(x => x.Network, model.Network)
                .Set(x => x.Nonce, model.Nonce)
                .Set(x => x.EphemeralPublicKey, model.EphemeralPublicKey)
                .Set(x => x.EphemeralKey, model.EphemeralKey)
                .Set(x => x.Randomness, model.Randomness)
                .Set(x => x.Epoch, model.Epoch)
                .Set(x => x.MaxEpoch, model.MaxEpoch)
                .Set(x => x.Url, model.Url)
                .Set(x => x.RefreshToken, model.RefreshToken)
                .Set(x => x.Token, model.Token)
                .Set(x => x.Address, model.Address)
                .Set(x => x.TimeStamp, DateTime.UtcNow);

            await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
        catch (Exception e)
        {
            throw new OauthProviderException($"Error inserting OAuth request: {e.Message}");
        }
    }

    public async Task UpdateAuth(ObjectId id, string token, string refreshToken)
    {
        var collection = await Get();
        var filter = Builders<OAuthRequestData>.Filter.Eq(m => m.Id, id);
        var update = Builders<OAuthRequestData>.Update
            .Set(x => x.Token, token)
            .Set(x => x.RefreshToken, refreshToken);
        await collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateAddress(ObjectId id, EnokiAddressData address)
    {
        var collection = await Get();
        var filter = Builders<OAuthRequestData>.Filter.Eq(m => m.Id, id);
        var update = Builders<OAuthRequestData>.Update
            .Set(x => x.Address, address.ToAddressData());
        await collection.UpdateOneAsync(filter, update);
    }

    public async Task<OAuthRequestData?> GetByNonce(string nonce)
    {
        var collection = await Get();
        var network = await configuration.SuiEnvironment;
        return await collection.Find(c => c.Nonce == nonce && c.Network == network).FirstOrDefaultAsync();
    }

    public async Task<OAuthRequestData?> GetById(string id)
    {
        var collection = await Get();
        return await collection.Find(c => c.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
    }

    public async Task<OAuthRequestData?> GetByGamer(long gamerTag, string network, string nameSpace)
    {
        var collection = await Get();
        var data = await collection.Find(c => c.GamerTag == gamerTag && c.Network == network && c.Namespace == nameSpace).ToListAsync();
        if (data is null || data.Count == 0) return null;
        return data.LastOrDefault();
    }
}