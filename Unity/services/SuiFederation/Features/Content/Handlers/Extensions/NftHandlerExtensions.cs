using System.Linq;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Content.Models;
using Beamable.SuiFederation.Features.Inventory.Models;

namespace Beamable.SuiFederation.Features.Content.Handlers.Extensions;

public static class NftHandlerExtensions
{
    private static string FindAttachment(this InventoryRequestUpdate inventoryRequestUpdate)
        => inventoryRequestUpdate.Properties.FirstOrDefault(kv => kv.Key.StartsWithFast("$attach")).Value ?? "";

    private static string FindDetachment(this InventoryRequestUpdate inventoryRequestUpdate)
        => inventoryRequestUpdate.Properties.FirstOrDefault(kv => kv.Key.StartsWithFast("$detach")).Value ?? "";

    public static (string, string) ResolveUpdateActions(this InventoryRequestUpdate inventoryRequestUpdate)
    {
        var attachment = FindAttachment(inventoryRequestUpdate);
        var detachment = FindDetachment(inventoryRequestUpdate);

        return (attachment, detachment) switch
        {
            // Case 1: An attachment exists (we don't care about detachment here).
            var (att, _) when !string.IsNullOrWhiteSpace(att)
                => (att, FunctionMessageNames.NftAttach),

            // Case 2: No attachment, but a detachment exists.
            var (_, det) when !string.IsNullOrWhiteSpace(det)
                => (det, FunctionMessageNames.NftDetach),

            // Case 3: Default when neither exists.
            _ => (string.Empty, FunctionMessageNames.NftUpdate)
        };
    }

}