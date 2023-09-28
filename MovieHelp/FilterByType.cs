using System.Text.RegularExpressions;

namespace FrizzLib.MovieHelp;

public static class FilterByType
{
    public static List<MovieOrEpisode> Video(IEnumerable<FileInfo> fileInfos)
    {
        return (from file in fileInfos
                where Regex.IsMatch(file.Extension.ToLower(), "avi|mkv|mp4")
                select new MovieOrEpisode(file)).ToList();
    }
    public static List<Subtitle> Subtitle(IEnumerable<FileInfo> fileInfos)
    {
        return (from file in fileInfos
                where Regex.IsMatch(file.Extension.ToLower(), "srt|ass")
                select new Subtitle(file)).ToList();
    }
}