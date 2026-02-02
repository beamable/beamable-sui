namespace Beamable.SuiFederation.Features.SuiApi.Models;

public readonly record struct CreateWalletResponse(string Address, string PrivateKey, string PublicKey);