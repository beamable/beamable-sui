using System;

namespace Beamable.SuiFederation.Extensions;

public static class StringExtensions
{
    public static bool StartsWithFast(this string value, string prefix, StringComparison comparisonType = StringComparison.Ordinal)
    {
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(prefix))
            return false;
        return value.StartsWith(prefix, comparisonType);
    }

    public static long ToLong(this string value)
        => long.TryParse(value, out var result) ? result : 0;

    public static StringSplitEnumerator SplitWithEnumerator(this string input, char separator)
        => new (input.AsSpan(), separator);

}