using System.Text.Json;

namespace Beamable.SuiFederation.Features.Content.FunctionMessages;

public record PersonalKioskCreateMessage(
    string ContentId,
    string PackageId,
    string Module,
    string Function,
    string PlayerWalletAddress,
    string PlayerWalletKey,
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

public record BaseRulePackageIds(string? royaltyRulePackageId, string? kioskLockRulePackageId,string? personalKioskRulePackageId,string? floorPriceRulePackageId);