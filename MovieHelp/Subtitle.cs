using System.Text.RegularExpressions;

namespace FrizzLib.MovieHelp;

/// <summary>
/// Class to help with processing of subtitle files using <em>mkvmerge.exe</em>.
/// </summary>
public class Subtitle
{
    /// <summary>
    /// The ISO639-2 language code (e.g. esp for Spanish)
    /// </summary>
    public string? LanguageCode { get; set; }       // ISO639-2 language code (e.g. esp)

    /// <summary>
    /// The long language description (e.g. Spanish)
    /// </summary>
    public string? LanguageString { get; set; }     // Long language description (e.g. Spanish)

    /// <summary>
    /// Whether English had to be assumed due to lack of information in the file name.
    /// </summary>
    public bool EnglishAssumed { get; set; }        // No language detected, so English assumed

    /// <summary>
    /// Whether this subtitle file has been matched with a video file.
    /// </summary>
    public bool MatchedToVideo { get; set; }

    /// <summary>
    /// Whether these are <strong>S</strong>ubtitles for the <strong>D</strong>eaf and <strong>H</strong>earing impaired.
    /// </summary>
    public bool IsSdh { get; set; } = false;        // Subtitles for the Deaf and Hearing impaired

    /// <summary>
    /// Whether these are <em>forced</em> subtitles.
    /// </summary>
    public bool IsForced { get; set; } = false;     // True if 'Forced' subtitle

    /// <summary>
    /// Reference to the subtitle file.
    /// </summary>
    public FileInfo Sub_FileInfo { get; private set; }

    /// <summary>
    /// Creates a Subtitle object from the specified subtitle file.
    /// Attempts to infer the subtitle language from the filename, but assumes English if this fails.
    /// </summary>
    /// <param name="Subtitle">Reference to the subtitle file.</param>
    // Constructor
    public Subtitle(FileInfo Subtitle)
    {
        Sub_FileInfo = Subtitle;
        string apparentLanguage = LanguageFromFilename(Sub_FileInfo.Name, out bool EnglAssumed);
        EnglishAssumed = EnglAssumed;
        LanguageCode = ISO639_Language.GetValidISO639_2Code(apparentLanguage);
        LanguageString = ISO639_Language.LongLanguage_FromISO639_2(LanguageCode);

        // Helper method
        static string LanguageFromFilename(string FileName, out bool EnglAssumed)
        // Returns "eng" if no language found in filename, else the language specified
        {
            string FileName_NoExt = Path.GetFileNameWithoutExtension(FileName);
            var match = Regex.Match(FileName_NoExt, @".*?([A-Za-z]{2,})$");
            string? lastWord = match.Groups[1].ToString();
            string? detectedLanguage = ISO639_Language.GetValidISO639_2Code(lastWord);
            EnglAssumed = false;
            // Assume English if no language detected
            if (detectedLanguage == null)
            {
                detectedLanguage = "eng";
                EnglAssumed = true;
            }
            return detectedLanguage;
        }
    }

    /// <summary>
    /// Method to determine if the specified video file matches this subtitle file.
    /// Examines the file and folder names and relative location to determine this.
    /// </summary>
    /// <param name="Video">The MovieOrEpisode object to compare to.</param>
    /// <returns><c>true</c> if video matches subtitle, else <c>false</c>.</returns>
    public bool BelongsTo(MovieOrEpisode Video)
    {
        var videoFileName = Path.GetFileNameWithoutExtension(Video.Video_FileInfo.Name);
        var subtitleFilename = Path.GetFileNameWithoutExtension(Sub_FileInfo.Name);
        var subtitleFolderName = Sub_FileInfo.Directory?.Name;
        if (videoFileName == subtitleFolderName) return true;
        if (videoFileName == subtitleFilename) return true;
        if (videoFileName == TrimFinalWord(subtitleFilename)) return true;
        if (SubIsInLevel1SubsFolder())
            if (LanguageCode != null && (NotAnEpisode() || EpisodesMatch())) return true;
        return false;

        #region Helper methods
        bool NotAnEpisode()
        {
            if (Regex.IsMatch(subtitleFilename, @"[ \.][Ee]\d\d[ \.]")) return false;
            else return true;
        }
        bool EpisodesMatch()
        {
            // Can't match episodes if no episode number in filenames
            if (!Regex.IsMatch(subtitleFilename, @"[ \.][Ee]\d\d([ \.]|$)")) return false;
            if (!Regex.IsMatch(videoFileName, @"[ \.][Ee]\d\d([ \.]|$)")) return false;

            //Check filenames match up to and including episode numbers
            var subtitle_Snip = Regex.Match(subtitleFilename, @"^(.*[ \.][Ee]\d\d([ \.]|$))").Groups[1].ToString();
            var video_Snip = Regex.Match(videoFileName, @"^(.*[ \.][Ee]\d\d([ \.]|$))").Groups[1].ToString();
            if (subtitle_Snip == video_Snip) return true;
            else return false;
        }
        string TrimFinalWord(string name)
        {
            // Remove extention
            var trimmedFilename = Regex.Match(name, @"(.*)[ _.-][A-Za-z]{2,}$").Groups[1].ToString();
            return trimmedFilename;
        }
        bool SubIsInLevel1SubsFolder()
        {
            var subPath = Sub_FileInfo.Directory?.FullName;
            var videoPath = Video.Video_FileInfo.Directory?.FullName;
            if (subPath == null || videoPath == null) return false;
            // if (videoPath == subPath) return false;
            var subPathDepth = Regex.Matches(subPath, @"\\").Count;
            var videoDepth = Regex.Matches(videoPath, @"\\").Count;
            if (subPathDepth - videoDepth != 1) return false;   // Video folder not an immediate parent of subtitle
            if (!subPath.Contains(videoPath)) return false;     // Subtitle doesn't have video as an ancestor
            var subtitleParentFolder = subPath[(videoPath.Length + 1)..].ToUpper();
            return Regex.IsMatch(subtitleParentFolder, @"\SUBS|\SUBTITLES");
        }
        #endregion
    }

    /// <summary>
    /// Generates the <em>mkvmerge.exe</em> command-line segment for this subtitle file.
    /// Appropriate switches are included for language and SDH and forced flags.
    /// </summary>
    /// <returns>The <em>mkvmerge.exe</em> command-line segment.</returns>
    public string MkvCommandSnippet()
    {
        string mkvSnippet = " --language 0:" + LanguageCode + " --default-track-flag 0:0";
        mkvSnippet += IsSdh ? " --hearing-impaired-flag 0 --track-name 0:SDH" : "";
        mkvSnippet += IsForced ? " --forced-display-flag 0 --track-name 0:forced" : "";
        mkvSnippet += " \"" + Sub_FileInfo.FullName + "\"";
        return mkvSnippet;
    }
}