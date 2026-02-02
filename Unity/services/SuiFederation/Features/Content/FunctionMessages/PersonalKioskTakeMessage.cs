using System.Text.Json;

namespace Beamable.SuiFederation.Features.Content.FunctionMessages;

public record PersonalKioskTakeMessage(
    string ContentId,
    string PackageId,
    string Module,
    string Function,
    string PlayerWalletAddress,
    string PlayerWalletKey,
    string KioskId,
    string PersonalKioskCap,
    string ItemType,
    string ItemId,
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