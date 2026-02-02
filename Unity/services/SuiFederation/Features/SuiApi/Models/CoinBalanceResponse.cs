namespace Beamable.SuiFederation.Features.SuiApi.Models;

public readonly record struct CoinBalanceResponse(
    string CoinType,
    long Total);