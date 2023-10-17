using System.Collections;
using static System.Math;

namespace FrizzLib.Misc;

/// <summary>
/// Generates prime numbers within a specified range.
/// </summary>
public class PrimeNumberGenerator : IEnumerable<int>
{
    private static readonly List<int> primeDivisors;     // Initialized in static constructor
    private static int maxPrimeDivisor;         // Initialized in static constructor
    private readonly int startingFrom;
    private readonly int endingAt;

    #region static constructor
    // Initialize static variables
    static PrimeNumberGenerator() {
        primeDivisors = new List<int> { 2, 3 };
        maxPrimeDivisor = 3;
    }
    #endregion

    #region instance constructor
    /// <summary>
    /// Once an instance of the class is created, the user can enumerate over the range of prime numbers.
    /// </summary>
    /// <param name="StartingFrom"></param>
    /// <param name="EndingAt"></param>
    /// <example>
    /// <code>
    /// PrimeNumberGenerator primeNumbers = new(500, 1000);
    /// Console.WriteLine("Listing prime numbers between 500 and 1000:");
    /// foreach (int prime in primeNumbers)
    ///     Console.Write($"{prime}, ");
    /// </code>
    /// </example>
    public PrimeNumberGenerator(int StartingFrom, int EndingAt) {
        startingFrom = StartingFrom;
        endingAt = EndingAt;
    }
    #endregion

    public IEnumerator<int> GetEnumerator() {
        int maxDivisorRequired = (int)Sqrt(endingAt);
        if (maxDivisorRequired > maxPrimeDivisor) AddPrimeDivisorsUpTo(maxDivisorRequired);
        if (endingAt < 2) yield break;                              // Return no prime numbers (can't end at less than 2)
        int firstIntToTest = startingFrom < 2 ? 2 : startingFrom;   // First prime must be at least 2
        if (firstIntToTest == 2) yield return firstIntToTest;       // If first prime required is 2, return it
        if (endingAt < 3) yield break;                              // Don't continue if primes above 2 have not been requested
        int testInt = firstIntToTest;
        if (testInt.IsEven()) testInt++;                             // Start testing at an odd number
        if (testInt > endingAt) yield break;                        // Return nothing if lower exceeds upper limit after incrementing
        do {
            if (IsPrime(testInt)) yield return testInt;
        }
        while ((testInt += 2) <= endingAt);
    }

    private static bool IsPrime(int currentInteger) {
        int sqrtCurrentInteger = (int)Sqrt(currentInteger);
        foreach (int divisor in primeDivisors) {
            if (divisor > sqrtCurrentInteger) break;
            if (currentInteger % divisor == 0) return false;
        }
        return true;
    }

    private static void AddPrimeDivisorsUpTo(int TopLimit) {
        var pg = new PrimeNumberGenerator(maxPrimeDivisor + 2, TopLimit);
        foreach (int prime in pg) {
            primeDivisors.Add(prime);
            maxPrimeDivisor = prime;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
