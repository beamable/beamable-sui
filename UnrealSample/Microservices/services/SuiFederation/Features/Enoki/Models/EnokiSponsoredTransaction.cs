using System.Linq;
using System.Text.Json.Serialization;

namespace Beamable.SuiFederation.Features.Enoki.Models;

public readonly record struct EnokiSponsoredTransactionCreateApiRequest(
    [property: JsonPropertyName("network")] string SuiNetwork,
    [property: JsonPropertyName("transactionBlockKindBytes")] string TransactionBlockKindBytes,
    [property: JsonPropertyName("allowedAddresses")] string[] AllowedAddresses,
    [property: JsonPropertyName("allowedMoveCallTargets")] string[] AllowedMoveCallTargets);

public readonly record struct EnokiSponsoredTransactionCreate(
    [property: JsonPropertyName("data")] EnokiSponsoredTransactionCreateData CreateData);

public readonly record struct EnokiSponsoredTransactionCreateData(
    [property: JsonPropertyName("digest")] string Digest,
    [property: JsonPropertyName("bytes")] string Bytes
);

public readonly record struct EnokiSponsoredTransactionCreateRequest(
    string SuiNetwork,
    string Jwt,
    string TransactionBlockKindBytes,
    string PlayerWalletAddress,
    EnokiSponsoredTransactionRequestFunction[] Functions
)
{
    public EnokiSponsoredTransactionCreateApiRequest ToApiRequest()
    {
        return new EnokiSponsoredTransactionCreateApiRequest(
            SuiNetwork,
            TransactionBlockKindBytes,
            [PlayerWalletAddress],
            Functions.Select(x => $"{x.PackageId}::{x.Module}::{x.Function}").ToArray());
    }
}

public readonly record struct EnokiSponsoredTransactionRequestFunction(
    string PackageId,
    string Module,
    string Function
);

public readonly record struct EnokiSponsoredTransactionExecuteApiRequest(
    [property: JsonPropertyName("signature")] string Signature);

public readonly record struct EnokiSponsoredTransactionExecuteRequest(
    string Digest,
    string Signature)
{
    public EnokiSponsoredTransactionExecuteApiRequest ToApiRequest()
    {
        return new EnokiSponsoredTransactionExecuteApiRequest(Signature);
    }
}

public readonly record struct EnokiSponsoredTransactionExecute(
    [property: JsonPropertyName("data")] EnokiSponsoredTransactionCreateData CreateData);

public readonly record struct EnokiSponsoredTransactionExecuteData(
    [property: JsonPropertyName("digest")] string Digest
);