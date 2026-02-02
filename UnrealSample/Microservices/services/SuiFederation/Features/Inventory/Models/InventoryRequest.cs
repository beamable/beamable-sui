using System.Collections.Immutable;
using SuiFederationCommon.Extensions;

namespace Beamable.SuiFederation.Features.Inventory.Models;

public readonly record struct InventoryRequest(
    long GamerTag,
    string ContentId,
    long Amount,
    ImmutableDictionary<string, string> Properties);


public static class InventoryRequestExtensions
{
    public static string ToNftType(this InventoryRequest inventoryRequest)
        => inventoryRequest.ContentId.ToContentType();
}