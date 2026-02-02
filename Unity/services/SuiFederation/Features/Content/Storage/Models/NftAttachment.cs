using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Beamable.SuiFederation.Features.Content.Storage.Models;

public record NftAttachment(
    [property: BsonId] ObjectId Id,
    string ParentProxy,
    string ParentContentId,
    string AttachmentContentId,
    string AttachmentName,
    string[] AttachmentObjects
)
{
    public static NftAttachment Create(string parentProxy, string parentContentId, string attachmentContentId, string attachmentName, string[] attachmentObjects)
        => new(ObjectId.GenerateNewId(), parentProxy, parentContentId, attachmentContentId, attachmentName, attachmentObjects);
}