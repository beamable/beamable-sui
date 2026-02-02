using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Beamable.SuiFederation.Features.Content.FunctionMessages;

public record KioskDelistMessage(
    string ContentId,
    string PackageId,
    string Module,
    string Function,
    string PlayerWalletAddress,
    string MarketplaceId,
    string ListingId,
    string PlayerWalletKey)
    : BaseMessage(ContentId, PackageId, Module, Function, PlayerWalletAddress)
{
    public string SerializeSelected()
    {
        var selectedData = new
        {
            PackageId,
            Module,
            Function,
            PlayerWalletAddress,
            MarketplaceId,
            ListingId
        };

        return JsonSerializer.Serialize(selectedData);
    }
}

public static class KioskDelistMessageExtensions
{
    public static string SerializeSelected(this List<KioskDelistMessage> messages)
        => string.Join(",",messages.Select(m => m.SerializeSelected()));
}