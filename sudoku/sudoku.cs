using System.Text.RegularExpressions;

namespace FrizzLib.Sudoku;

/// <summary>A static class with methods to help solve Sudoku puzzles.
/// <para><u>Available methods</u></para>
/// <list type="bullet">
/// <item><see cref="GetCombos(int, int)"/></item>
/// <item><see cref="FilterOutCombosWithTheseDigits(List{string}, string)"/></item>
/// </list>
/// </summary>
public static class Sudoku
{
    #region Public methods
    /// <summary>Determines all possible combinations of digits given the sum of the digits and the number of digits.</summary>
    /// <param name="totalReqd">The sum the digits must add to.</param>
    /// <param name="cellsAvail">The number of digits to add together.</param>
    /// <returns>A list of strings, each containing a possible combination.  The list will be empty (have zero members) if no combinations are possible with the given constraints.</returns>
    /// <remarks>In Sudoku the only digits allowed are the digits 1-9, and none may be repeated in a row, column, 3x3 block, or cage (in Sum Sudoku).</remarks>
    public static List<string> GetCombos(int totalReqd, int cellsAvail) => GetCombos(totalReqd, cellsAvail, 1);

    /// <summary>Takes a list of digit combinations and removes any that include specified digits.</summary>
    /// <param name="Combos">The list of combinations to process.</param>
    /// <param name="nonAllowedDigits">The digits that are not allowed to occur in a combination.</param>
    /// <returns>A new list containing only the combinations that do not contain non-allowed digits.</returns>
    /// <remarks>In Sum Sudoku some digits may not be allowed in a cage due to the fact they already appear in an intersecting column, row or 3x3 block.</remarks>
    public static List<string> FilterOutCombosWithTheseDigits(List<string> Combos, string nonAllowedDigits)
    {
        return Combos.Where(i => !nonAllowedDigits.Any(j => i.Contains(j))).ToList<string>();
    }
    #endregion

    private static List<string> GetCombos(int totalReqd, int cellsAvail, int baseValue)
    {
        List<string> futureCombos;
        List<string> currentCombos = new();

        if (totalReqd < baseValue || totalReqd > 45 || cellsAvail > 9) return currentCombos;
        if (cellsAvail == 1)
        {
            if (totalReqd > 9) return currentCombos;
            else return new List<string>() { totalReqd.ToString() };
        }
        else
        {
            for (int thisDigit = baseValue; thisDigit < 9; thisDigit++)
            {
                futureCombos = GetCombos(totalReqd - thisDigit, cellsAvail - 1, thisDigit + 1);
                if (futureCombos.Count == 0) continue;
                else foreach (string s in futureCombos) currentCombos.Add(thisDigit.ToString() + s);
            }
            return currentCombos;
        }
    }
}
