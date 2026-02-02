using System.Collections.Immutable;
using SuiFederationCommon.Extensions;

namespace Beamable.SuiFederation.Features.Inventory.Models;

public readonly record struct InventoryRequestUpdate(
    long GamerTag,
    string ContentId,
    string ProxyId,
    ImmutableDictionary<string, string> Properties);


public static class InventoryRequestUpdateExtensions
{
    public static string ToNftType(this InventoryRequestUpdate inventoryRequest)
        => inventoryRequest.ContentId.ToContentType();
}