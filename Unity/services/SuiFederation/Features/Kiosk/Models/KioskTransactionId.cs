using System;
using Beamable.SuiFederation.Extensions;
using MongoDB.Bson;

namespace Beamable.SuiFederation.Features.Kiosk.Models;

public readonly struct KioskTransactionId
{
    private string TransactionId { get; }
    private Guid UniqueId { get; }

    private static string Prefix => "KT";

    public KioskTransactionId()
    {
        TransactionId = ObjectId.GenerateNewId().ToString();
        UniqueId = Guid.NewGuid();
    }

    private KioskTransactionId(string transactionId, Guid uniqueId)
    {
        TransactionId = transactionId;
        UniqueId = uniqueId;
    }

    public static bool TryParse(string transactionIdString, out KioskTransactionId kioskTransactionId)
    {
        kioskTransactionId = default;

        if (string.IsNullOrEmpty(transactionIdString))
            return false;

        var enumerator = transactionIdString.SplitWithEnumerator('-');
        if (!enumerator.MoveNext())
            return false;

        var firstPart = enumerator.Current;
        if (!enumerator.MoveNext())
            return false;
        var secondPart = enumerator.Current;
        if (!enumerator.MoveNext())
            return false;
        var thirdPart = enumerator.Current;
        if (enumerator.MoveNext())
            return false;

        if (firstPart != Prefix)
            return false;

        if (!Guid.TryParse(thirdPart, out var guid))
            return false;

        kioskTransactionId = new KioskTransactionId(secondPart.ToString(), guid);
        return true;
    }

    public static implicit operator string(KioskTransactionId transaction) => $"{Prefix}-{transaction.TransactionId}-{transaction.UniqueId:N}";
    public override string ToString() => $"{Prefix}-{TransactionId}-{UniqueId:N}";
}