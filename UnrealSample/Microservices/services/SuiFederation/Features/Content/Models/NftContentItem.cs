using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Inventory.Models;
using SuiFederationCommon.Models;
using SuiFederationCommon.Models.Kiosk;

namespace Beamable.SuiFederation.Features.Content.Models;

public readonly record struct NftContentItem(
    string Name,
    string Url,
    string Description,
    string ContentId,
    ImmutableArray<NftAttribute> Attributes)
{
    public static NftContentItem Empty => new ("", "", "", "", ImmutableArray<NftAttribute>.Empty);
}

public readonly record struct NftAttribute(string Name, string Value);

public static class NftContentItemExtensions
{
    public static NftContentItem Create(InventoryRequest inventoryRequest, INftBase nftBase)
    {
        var requestProperties = inventoryRequest.Properties.IsEmpty ? ImmutableDictionary<string, string>.Empty : inventoryRequest.Properties;
        var name = requestProperties.FirstOrDefault(kv => kv.Key.StartsWith("$name", StringComparison.OrdinalIgnoreCase)).Value ?? nftBase.Name;
        var image = requestProperties.FirstOrDefault(kv => kv.Key.StartsWith("$image", StringComparison.OrdinalIgnoreCase)).Value ?? nftBase.Image;
        var description = requestProperties.FirstOrDefault(kv => kv.Key.StartsWith("$description", StringComparison.OrdinalIgnoreCase)).Value ?? nftBase.Description;
        var attributes = GetAttributes(requestProperties, nftBase.CustomProperties.ToImmutableDictionary());
        return new NftContentItem(name,image,description,inventoryRequest.ContentId, attributes.Select(kv => new NftAttribute(kv.Key, kv.Value)).ToImmutableArray());
    }

    public static NftContentItem Create(string contentId, INftBase nftBase)
    {
        return new NftContentItem(nftBase.Name,nftBase.Image, nftBase.Description, contentId, nftBase.CustomProperties.Select(kv => new NftAttribute(kv.Key, kv.Value)).ToImmutableArray());
    }

    public static NftContentItem CreateForUpdate(string contentId, INftBase nftBase, List<NftAttribute> attributes)
    {
        return new NftContentItem(nftBase.Name,nftBase.Image, nftBase.Description, contentId, attributes.ToImmutableArray());
    }

    private static Dictionary<string, string> GetAttributes(ImmutableDictionary<string, string> dynamicProperties, ImmutableDictionary<string, string> staticProperties)
    {
        var filteredDynamic = dynamicProperties
            .Where(kvp => kvp.Key.StartsWithFast("$") &&
                          !kvp.Key.StartsWithFast("$name") &&
                          !kvp.Key.StartsWithFast("$image") &&
                          !kvp.Key.StartsWithFast("$description"))
            .ToDictionary(kvp => kvp.Key.TrimStart('$'), kvp => kvp.Value);
        var result = new Dictionary<string, string>(staticProperties);
        foreach (var kvp in filteredDynamic)
        {
            result[kvp.Key] = kvp.Value;
        }
        return result;
    }

    public static Dictionary<string, string> GetAttributes(this NftContentItem item)
    {
        var result = new Dictionary<string, string>
        {
            { nameof(item.Name), item.Name },
            { nameof(item.Description), item.Description },
            { nameof(item.Url), item.Url }
        };
        foreach (var kvp in item.Attributes)
        {
            result[kvp.Name] = kvp.Value;
        }
        return result;
    }

    public static ItemProperties[] ToItemProperties(this INftBase item)
    {
        var properties = new List<ItemProperties>
        {
            new() { name = nameof(item.Name), value = item.Name },
            new() { name = nameof(item.Image), value = item.Image },
            new() { name = nameof(item.Description), value = item.Description }
        };
        properties.AddRange(item.CustomProperties.Select(attr =>
            new ItemProperties { name = attr.Key, value = attr.Value }));
        return properties.ToArray();
    }

    public static HashSet<string> FixedProperties()
        => ["name", "image", "description", "type"];
}