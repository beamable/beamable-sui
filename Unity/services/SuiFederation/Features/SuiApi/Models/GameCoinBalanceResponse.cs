namespace Beamable.SuiFederation.Features.SuiApi.Models;

public readonly record struct GameCoinBalanceResponse(
    string CoinType,
    long Total);