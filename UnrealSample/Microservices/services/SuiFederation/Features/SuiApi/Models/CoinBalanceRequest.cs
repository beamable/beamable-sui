namespace Beamable.SuiFederation.Features.SuiApi.Models;

public readonly record struct CoinBalanceRequest(
    string PackageId,
    string ModuleName);