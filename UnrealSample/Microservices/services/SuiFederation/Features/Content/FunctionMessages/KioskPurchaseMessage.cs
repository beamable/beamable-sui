using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Beamable.SuiFederation.Features.Content.FunctionMessages;

public record KioskPurchaseMessage(
    string ContentId,
    string PackageId,
    string Module,
    string Function,
    string PlayerWalletAddress,
    string MarketplaceId,
    string ListingId,
    string CurrencyPackageId,
    string CurrencyModule,
    long Price,
    string TokenPolicy,
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

public static class KioskPurchaseMessageExtensions
{
    public static string SerializeSelected(this List<KioskPurchaseMessage> messages)
        => string.Join(",",messages.Select(m => m.SerializeSelected()));
}