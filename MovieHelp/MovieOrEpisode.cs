using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FrizzLib.MovieHelp;

/// <summary>
/// Class to help with manipulating video files using mkvmerge.exe
/// </summary>
public class MovieOrEpisode
{
    public FileInfo Video_FileInfo { get; set; }
    public string Title { get; set; }
    public List<Subtitle> Subtitles { get; set; }
    static Process? Muxer;

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

    public string MkvCommandSnippet()
    {
        return MkvCommandSnippet("eng");
    }

    public string MkvCommandSnippet(string SpokenLanguageCode)
    {
        string title = $"--title \"{Title}\"";
        string quietTag = " -q";
        string videoLanguage = $" --language 0:{SpokenLanguageCode} --language 1:{SpokenLanguageCode}";
        string outputFile = $" -o \"{Video_FileInfo.DirectoryName}\\Remuxed\\{Title}.drMUX.mkv\"";
        string inputFile = $" \"{Video_FileInfo.FullName}\"";
        return title + quietTag + videoLanguage + outputFile + inputFile;
    }

    public static async Task<string> MuxThisAsync(string mkvArgs)
    {
        ProcessStartInfo startInfo;
        String MuxerExitStatus = "unknown";
        System.Console.Write($"MUXing now ");
        try
        {
            startInfo = new ProcessStartInfo(@"C:\Users\David\Local Progs\MkvToolNix\mkvmerge.exe");
            startInfo.UseShellExecute = false;
            startInfo.Arguments = mkvArgs;
            using (Muxer = Process.Start(startInfo))
            {
                if (Muxer != null)
                {
                    await Muxer.WaitForExitAsync();
                    switch (Muxer.ExitCode)
                    {
                        case 0:
                        default:
                            MuxerExitStatus = "MUXing successful";
                            break;
                        case 1:
                            MuxerExitStatus = "MUXer completed with WARNING(S)";
                            break;
                        case 2:
                            MuxerExitStatus = "ERROR - MUXing aborted";
                            break;
                    }
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