using System.Collections.Generic;
using System.Threading.Tasks;
using Beamable.Server;
using Beamable.SuiFederation.Features.Content.Storage.Models;
using MongoDB.Driver;

namespace Beamable.SuiFederation.Features.Content.Storage;

public class NftAttachmentCollection(IStorageObjectConnectionProvider storageObjectConnectionProvider)
    : IService
{
    private IMongoCollection<NftAttachment>? _collection;

    private async ValueTask<IMongoCollection<NftAttachment>> Get()
    {
        if (_collection is not null) return _collection;
        _collection =
            (await storageObjectConnectionProvider.SuiFederationStorageDatabase()).GetCollection<NftAttachment>("nft-attachments");
        await _collection.Indexes.CreateManyAsync([
            new CreateIndexModel<NftAttachment>(Builders<NftAttachment>.IndexKeys.Ascending(x => x.ParentProxy)),
            new CreateIndexModel<NftAttachment>(Builders<NftAttachment>.IndexKeys
                .Ascending(x => x.ParentProxy)
                .Ascending(x => x.AttachmentContentId))
        ]);
        return _collection;
    }

    public async Task<bool> TryInsert(NftAttachment attachment)
    {
        var collection = await Get();
        try
        {
            await collection.InsertOneAsync(attachment);
            return true;
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }

    public async Task Insert(IEnumerable<NftAttachment> attachments)
    {
        var collection = await Get();
        var options = new InsertManyOptions
        {
            IsOrdered = false
        };
        await collection.InsertManyAsync(attachments, options);
    }

    public async Task<List<NftAttachment>> GetByParentIds(IEnumerable<string> parentIds)
    {
        var collection = await Get();
        var filter = Builders<NftAttachment>.Filter.In(attachment => attachment.ParentProxy, parentIds);
        return await collection.Find(filter).ToListAsync();
    }

    public async Task DeleteByParentIds(string parentId, string attachmentChildId)
    {
        var collection = await Get();
        var filter = Builders<NftAttachment>.Filter.And(
            Builders<NftAttachment>.Filter.Eq(m => m.ParentProxy, parentId),
            Builders<NftAttachment>.Filter.Eq(m => m.AttachmentContentId, attachmentChildId)
        );
        await collection.DeleteManyAsync(filter);
    }

    public async Task<List<NftAttachment>> FindByAttachmentObject(IEnumerable<string> attachmentId)
    {
        var collection = await Get();
        var filter = Builders<NftAttachment>.Filter.AnyIn(x => x.AttachmentObjects, attachmentId);
        return await collection.Find(filter).ToListAsync();
    }
}