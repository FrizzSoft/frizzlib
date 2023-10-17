namespace FrizzLib.Misc;

/// <summary>
/// Various useful extension methods
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Determines if an int is an odd number.
    /// </summary>
    /// <param name="i">The int to examine.</param>
    /// <returns><c>true</c> if <c>i</c> is odd, else <c>false</c></returns>
    public static bool IsOdd(this int i) => i % 2 != 0;

    /// <summary>
    /// Determines if an int is an even number.
    /// </summary>
    /// <param name="i">The int to examine.</param>
    /// <returns><c>true</c> if <c>i</c> is even, else <c>false</c></returns>
    public static bool IsEven(this int i) => i % 2 == 0;
}
