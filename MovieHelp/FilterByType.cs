using System.Text.RegularExpressions;

namespace FrizzLib.MovieHelp;

/// <summary>
/// From the supplied files, separate out either videos or subtitles.
/// </summary>
public static class FilterByType
{
    /// <summary>
    /// Select the <em>video</em> files from the files provided, and return them as a list.
    /// </summary>
    /// <param name="fileInfos">The IEnumerable of files to parse.</param>
    /// <returns>A list of <em>video</em> files.</returns>
    public static List<MovieOrEpisode> Video(IEnumerable<FileInfo> fileInfos)
    {
        return (from file in fileInfos
                where Regex.IsMatch(file.Extension.ToLower(), "avi|mkv|mp4")
                select new MovieOrEpisode(file)).ToList();
    }

    /// <summary>
    /// Select the <em>subtitle</em> files from the files provided, and return them as a list.
    /// </summary>
    /// <param name="fileInfos">The IEnumerable of files to parse.</param>
    /// <returns>A list of <em>subtitle</em> files.</returns>
    public static List<Subtitle> Subtitle(IEnumerable<FileInfo> fileInfos)
    {
        return (from file in fileInfos
                where Regex.IsMatch(file.Extension.ToLower(), "srt|ass")
                select new Subtitle(file)).ToList();
    }
}