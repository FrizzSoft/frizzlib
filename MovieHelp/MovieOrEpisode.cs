using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FrizzLib.MovieHelp;
/// <summary>
/// A class to assist with the multiplexing of movies using the mkvtoolnix mkvmerge.exe tool.
/// </summary>
public class MovieOrEpisode
{
    /// <summary>
    /// Reference to the current video file.
    /// </summary>
    public FileInfo Video_FileInfo { get; set; }

    /// <summary>
    /// The title of the video, extracted from the filename on construction.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The subtitles associated with this video file.
    /// </summary>
    public List<Subtitle> Subtitles { get; set; }

    static Process? Muxer;

    /// <summary>
    /// Creates a MovieOrEpisode instance from the specified video file.
    /// Tries to infer the movie or episode <c>Title</c> from the filename.
    /// </summary>
    /// <param name="file">Specifies the source video file.</param>
    /// <remarks>The following exception should not be thrown, as a FileInfo must contain a Name property.</remarks>
    /// <exception cref="ArgumentException">A null or empty filename will not be accepted.</exception>
    // Constructor
    public MovieOrEpisode(FileInfo file)
    {
        Video_FileInfo = file;
        Title = ExtractTitle(file.Name);
        Subtitles = new List<Subtitle>();

        // Helper method
        static string ExtractTitle(string Filename)
        {
            if (string.IsNullOrEmpty(Filename)) throw new ArgumentException("Argument null or empty string.");
            // Separate filename from extension
            string fileName = Path.GetFileNameWithoutExtension(Filename);
            // Truncate text after the Season and Episode id's
            var match = Regex.Match(fileName, @"(.*[Ss]\d\d[Ee]\d\d)");
            if (!match.Success) return fileName;
            else
            {
                // Change periods to spaces, unless in form digit.digit, e.g. 2.0
                fileName = Regex.Replace(match.Groups[1].Value, @"(?<=\D)\.|\.(?=\D)", " ");
                // Make Season and Episode letters upper case for clarity/consistency
                fileName = Regex.Replace(fileName, @"([Ss])(?=\d\d[Ee]\d\d)", "S");
                fileName = Regex.Replace(fileName, @"(?<=[Ss]\d\d)([Ee])(?=\d\d)", "E");
                return fileName.Trim();  // Remove leading or trailing whitespace
            }
        }
    }

    /// <summary>
    /// Generates the mkvmerge.exe command segment for this movie file, assuming the spoken language to be English.
    /// Later, additional segments will be appended for each subtitle file,
    /// the result being the complete command-line string to pass to mkvmerge.exe.
    /// </summary>
    /// <returns>The generated command segment.</returns>
    public string MkvCommandSnippet()
    {
        return MkvCommandSnippet("eng");
    }

    /// <summary>
    /// Generates the mkvmerge.exe command segment for this movie file,
    /// inserting the appropriate language code for the specified spoken language.
    /// Later, additional segments will be appended for each subtitle file,
    /// the result being the complete command-line string to pass to mkvmerge.exe.
    /// </summary>
    /// <param name="SpokenLanguageCode">The spoken language of the video.</param>
    /// <returns>The generated command segment.</returns>
    public string MkvCommandSnippet(string SpokenLanguageCode)
    {
        string title = $"--title \"{Title}\"";
        string quietTag = " -q";
        string videoLanguage = $" --language 0:{SpokenLanguageCode} --language 1:{SpokenLanguageCode}";
        string outputFile = $" -o \"{Video_FileInfo.DirectoryName}\\Remuxed\\{Title}.drMUX.mkv\"";
        string inputFile = $" \"{Video_FileInfo.FullName}\"";
        return title + quietTag + videoLanguage + outputFile + inputFile;
    }

    /// <summary>
    /// Runs an async instance of the mkvtoolnix.exe command line tool, to multiplex the current video.
    /// </summary>
    /// <param name="mkvArgs">The command-line string to pass to mkvtoolnix.exe.</param>
    /// <returns>The status at termination of the multiplexing operation (success or otherwise).</returns>
    public static async Task<string> MuxThisAsync(string mkvArgs)
    {
        ProcessStartInfo startInfo;
        String MuxerExitStatus = "unknown";
        System.Console.Write($"MUXing now ");
        try
        {
            startInfo = new ProcessStartInfo(@"C:\Users\David\Local Progs\MkvToolNix\mkvmerge.exe")
            {
                UseShellExecute = false,
                Arguments = mkvArgs
            };
            using (Muxer = Process.Start(startInfo))
            {
                if (Muxer != null)
                {
                    await Muxer.WaitForExitAsync();
                    MuxerExitStatus = Muxer.ExitCode switch
                    {
                        1 => "MUXer completed with WARNING(S)",
                        2 => "ERROR - MUXing aborted",
                        _ => "MUXing successful",
                    };
                }
                return MuxerExitStatus;
            }
        }
        catch (Exception e)
        {
            return $"EXCEPTION: {e.Message}";
        }
    }
}