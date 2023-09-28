namespace FrizzLib.MovieHelp;

public class SubtitleLanguageComparer : IEqualityComparer<Subtitle>
{
    // Products are equal if their names and product numbers are equal.
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

    // If Equals() returns true for a pair of objects
    // then GetHashCode() must return the same value for these objects.

    public int GetHashCode(Subtitle sub)
    {
        //Check whether the object is null
        if (sub is null) return 0;

        //Get hash code for the Name field if it is not null.
        int hashLanguage = sub.LanguageCode?.GetHashCode() ?? 0;

        //Get hash code for the Code field.
        int hashProductCode = sub.Sub_FileInfo.Length.GetHashCode();

        //Calculate the hash code for the product.
        return hashLanguage ^ hashProductCode;
    }
}