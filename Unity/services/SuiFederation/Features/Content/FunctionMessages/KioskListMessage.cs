using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Beamable.SuiFederation.Features.Content.FunctionMessages;

public record KioskListMessage(
    string ContentId,
    string PackageId,
    string Module,
    string Function,
    string PlayerWalletAddress,
    string MarketplaceId,
    string ItemId,
    long Price,
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
            ItemId,
            Price
        };

        return JsonSerializer.Serialize(selectedData);
    }
}

public static class KioskListMessageExtensions
{
    public static string SerializeSelected(this List<KioskListMessage> messages)
        => string.Join(",",messages.Select(m => m.SerializeSelected()));
}