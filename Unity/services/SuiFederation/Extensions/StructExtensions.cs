namespace Beamable.SuiFederation.Extensions;

public static class StructExtensions
{
    public static bool IsEmpty<T>(this T value) where T : struct =>
        value.Equals(default(T));
}