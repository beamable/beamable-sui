using Beamable.SuiFederation.Features.Common;

namespace Beamable.SuiFederation.Features.WalletManager.Models;

public readonly record struct WalletCoinBalance(
    string Wallet,
    SuiAmount Balance);