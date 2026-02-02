using System;
using System.Collections.Generic;

namespace Beamable.SuiFederation.Features.SuiApi.Models;

public interface ISuiTransactionResult { }

public readonly record struct SuiTransactionResult(
    string status,
    string digest,
    string gasUsed,
    string[] objectIds,
    string error,
    string createdId,
    Dictionary<string, string>? metadata) : ISuiTransactionResult
{
    public bool IsSuccess => status.Equals("success", StringComparison.OrdinalIgnoreCase);
}

public readonly record struct SuiEnokiTransactionResult(
    string Id) : ISuiTransactionResult;