namespace Beamable.SuiFederation.Features.SuiApi.Models;

public readonly record struct GameCoinBalanceRequest(
    string PackageId,
    string ModuleName);