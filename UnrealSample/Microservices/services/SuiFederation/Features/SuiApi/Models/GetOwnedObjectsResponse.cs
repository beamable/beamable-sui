using System.Collections.Generic;
using System.Linq;
using Beamable.Common;
using Beamable.Common.Api.Inventory;
using Beamable.SuiFederation.Features.Content.Storage.Models;

namespace Beamable.SuiFederation.Features.SuiApi.Models;

public readonly record struct GetOwnedObjectsResponse(
    string ObjectId,
    string Type,
    string Name,
    string Description,
    string Image,
    string ContentId,
    ResponseAttribute[] Attributes);
public readonly record struct ResponseAttribute(string Name, string Value);

public static class GetOwnedObjectsResponseExtensions
{
    public static Dictionary<string, List<FederatedItemProxy>> ToInventoryState(this IEnumerable<GetOwnedObjectsResponse> response, List<NftAttachmentResult> attachmentResults)
    {
        var parentToChildLookup = attachmentResults.ToDictionary(ar => ar.ParentProxy, ar => ar.ChildProxy);
        var childToParentLookup = attachmentResults
            .SelectMany(ar => ar.ChildProxy.Select(child => new { child, ar.ParentProxy }))
            .ToDictionary(x => x.child, x => x.ParentProxy);

        return response
            .GroupBy(o => o.ContentId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(o =>
                {
                    var properties = new List<ItemProperty>
                        {
                            new() { name = "name", value = o.Name },
                            new() { name = "type", value = o.Type },
                            new() { name = "description", value = o.Description },
                            new() { name = "image", value = o.Image }
                        }
                        .Concat(o.Attributes.Select(a => new ItemProperty { name = a.Name, value = a.Value }))
                        .ToList();

                    // Check if there are attachments for the current proxyId (o.ObjectId) as parent
                    if (attachmentResults.Count != 0 && parentToChildLookup.TryGetValue(o.ObjectId, out var childProxies))
                    {
                        if (childProxies.Length != 0)
                        {
                            properties.Add(new ItemProperty { name = "attachments", value = string.Join(",", childProxies) });
                        }
                    }

                    // Check if there are attachments for the current proxyId (o.ObjectId) as child
                    if (attachmentResults.Count != 0 && childToParentLookup.TryGetValue(o.ObjectId, out var parentProxy))
                    {
                        if (!string.IsNullOrWhiteSpace(parentProxy))
                        {
                            properties.Add(new ItemProperty { name = "parent", value = parentProxy });
                        }
                    }

                    return new FederatedItemProxy
                    {
                        proxyId = o.ObjectId,
                        properties = properties
                    };
                }).ToList()
            );
    }
}