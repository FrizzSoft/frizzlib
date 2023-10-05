namespace FrizzLib.MovieHelp;

/// <summary>
/// A class to determine equality between 2 <see cref="Subtitle"/> objects.
/// </summary>
public class SubtitleLanguageComparer : IEqualityComparer<Subtitle>
{
    /// <summary>
    /// Compares 2 <see cref="Subtitle"/> objects to see if they are the same. They are
    /// considered equal if their language and the size of their associated files are equal.
    /// </summary>
    /// <param name="sub1">the first subtitle to compare</param>
    /// <param name="sub2">the second subtitle to compare</param>
    /// <returns><c>true</c> if the subtitles are the same, otherwise <c>false</c>.</returns>
    public bool Equals(Subtitle? sub1, Subtitle? sub2)
    {

        //Check whether the compared objects reference the same data.
        if (Object.ReferenceEquals(sub1, sub2)) return true;

        //Check whether any of the compared objects is null.
        if (sub1 is null || sub2 is null)
            return false;

        //Check whether the subtitles files have the same language and file size.
        return sub1.LanguageCode == sub2.LanguageCode && sub1.Sub_FileInfo.Length == sub2.Sub_FileInfo.Length;
    }

    /// <summary>
    /// Generate a hash code for the <see cref="Subtitle"/> object.
    /// </summary>
    /// <param name="sub"></param>
    /// <returns></returns>
    /// <remarks>If Equals() returns true for a pair of objects then
    /// GetHashCode() must return the same value for these objects.</remarks>
    public int GetHashCode(Subtitle sub)
    {
        // Check whether the object is null
        if (sub is null) return 0;

        // Get hash code for the Name field if it is not null.
        int hashLanguage = sub.LanguageCode?.GetHashCode() ?? 0;

        // Get hash code for the Code field.
        int hashProductCode = sub.Sub_FileInfo.Length.GetHashCode();

        // Calculate the hash code for the <see cref="Subtitle"/>.
        return hashLanguage ^ hashProductCode;
    }
}