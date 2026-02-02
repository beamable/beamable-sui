namespace Beamable.SuiFederation.Features.SuiApi.Models;

public interface ISuiTransactionResult { }

public readonly record struct SuiTransactionResult(
    string status,
    string digest,
    string gasUsed,
    string[] objectIds,
    string error,
    string createdId) : ISuiTransactionResult;

public readonly record struct SuiEnokiTransactionResult(
    string Id) : ISuiTransactionResult;