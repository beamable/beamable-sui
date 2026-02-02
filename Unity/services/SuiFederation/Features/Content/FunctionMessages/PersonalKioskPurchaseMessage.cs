using System.Text.Json;

namespace Beamable.SuiFederation.Features.Content.FunctionMessages;

public record PersonalKioskPurchaseMessage(
    string ContentId,
    string PackageId,
    string Module,
    string Function,
    string PlayerWalletAddress,
    string PlayerWalletKey,
    string KioskId,
    string SellerKioskId,
    string PersonalKioskCap,
    string ItemType,
    string ItemId,
    string PolicyId,
    long Price,
    string PurchaseCap,
    BaseRulePackageIds? CustomPackageIds)
    : BaseMessage(ContentId, PackageId, Module, Function, PlayerWalletAddress)
{
    public string SerializeSelected()
    {
        var selectedData = new
        {
            PackageId,
            Module,
            Function,
            PlayerWalletAddress
        };

        return JsonSerializer.Serialize(selectedData);
    }
}