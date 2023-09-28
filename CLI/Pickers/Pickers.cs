using System.Diagnostics;
using System.Text.RegularExpressions;
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

namespace FrizzLib.CLI.Pickers;
/// <summary>
/// Static class with methods that allow the user to select a folder using the Command Line Interface.
/// <para><u>Available methods</u></para>
/// <list type="bullet">
/// <item><see cref="PickFolder()"/> - Prompt user to pick a folder, with initial folder listed being the UserProfile folder.</item>
/// <item><see cref="PickFolder(string)"/> - Prompt user to pick a folder, with parameter <c>InitialPath</c> being the first folder listed.</item>
/// </list>
/// </summary>
public static class Pickers
{
    private static readonly string initialPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static readonly String[] logicalDrives = Environment.GetLogicalDrives();
    private static readonly string availableDrives;
    private static DirectoryInfo currentFolder = new(initialPath);
    private static readonly EnumerationOptions enumerationOptions = new()
    {
        // Initially skip System files, but include Hidden files
        AttributesToSkip = FileAttributes.System
    };

    // Static constructor
    static Pickers()
    {
        // Generate string (availableDrives) listing all available drives
        int drivesCount = logicalDrives.Length;
        availableDrives = logicalDrives[0].First().ToString();
        if (drivesCount > 2)
        {
            for (int i = 1; i < drivesCount - 1; i++)
            {
                availableDrives += "," + logicalDrives[i].First().ToString();
            }
        }
        if (logicalDrives.Length > 1)
            availableDrives += " or " + logicalDrives[drivesCount - 1].First().ToString();
    }

    /// <summary>
    /// Prompts user to pick a folder. When called with no arguments, listing starts in the UserProfile folder.
    /// </summary>
    /// <returns>A <c>DirectoryInfo</c> representing the folder picked, or null if no folder picked.</returns>
    public static FileInfo? PickFile()
    {
        return PickFile(initialPath);
    }

    /// <summary>
    /// Prompts user to pick a folder, specifying the initial folder to display.
    /// </summary>
    /// <param name="InitialPath"> - the initial folder to display</param>
    /// <returns>A <c>DirectoryInfo</c> representing the folder picked, or null if no folder picked.</returns>
    public static FileInfo? PickFile(string InitialPath)
    {
        FileSystemInfo_Picker fsiPicker = new()
        {
            HelpMessage = "<Enter> to continue if listing paused; (n)n to select file or folder;\n" +
        " .. for parent folder; ? for Help\n\nPlease enter your response: "
        };
        string? userResponse;
        int SelectedItemIndex;
        bool showHidden = true;
        FileSystemInfo[] fsiItemsArray;
        if (InitialPath == "Downloads") InitialPath = initialPath + @"\Downloads";
        if (!Directory.Exists(InitialPath))
            InitialPath = initialPath;      // Revert to UserProfile folder if Downloads not is std location
        currentFolder = new DirectoryInfo(InitialPath);
        FileInfo? pickedFile;

        do
        {
            fsiPicker.BatchHeading = currentFolder.FullName + (showHidden ? " (showing Hidden *H)" : "");
            fsiItemsArray = currentFolder.EnumerateFileSystemInfos("*", enumerationOptions).ToArray<FileSystemInfo>();
            userResponse = fsiPicker.PickItem(fsiItemsArray, 40);

            // Evaluate response
            if (userResponse == null)               // Return null (indicating no file selected)
            {
                pickedFile = null;
                break;
            }
            if (userResponse.ToUpper() == "H")      // Toggle Hidden folders
            {
                enumerationOptions.AttributesToSkip ^= FileAttributes.Hidden;
                showHidden = !showHidden;
                continue;
            }
            if (userResponse == "..")               // Change folder focus to parent folder
            {
                if (currentFolder.Parent == null)
                {
                    bool invalidResponse = true;
                    do
                    {
                        System.Console.Write($"Select an available Drive ({availableDrives}): ");
                        var Response = Console.ReadLine();
                        if (Response == "" || Response == null) continue;
                        var match = Regex.Match(Response, @"([a-zA-Z])($|:|:\\)");
                        if (!match.Success) continue;
                        string firstChar = match.Groups[1].Value.ToUpper();
                        if (!DriveExists(firstChar))
                        {
                            System.Console.WriteLine("Invalid drive specification");
                            continue;
                        }
                        currentFolder = new DirectoryInfo(firstChar + ":\\");
                        invalidResponse = false;
                    } while (invalidResponse);
                }
                else
                    currentFolder = currentFolder.Parent;
            }
            else                                    // Return the selected file, or change to selected folder
            {
                if (int.TryParse(userResponse, out SelectedItemIndex))
                {
                    var selectedItem = fsiItemsArray[SelectedItemIndex];
                    if (selectedItem is DirectoryInfo dirInfo)  // Change folder focus to selected numbered folder
                    {
                        currentFolder = dirInfo;
                        continue;
                    }
                    else                                        // Return picked file
                    {
                        pickedFile = selectedItem as FileInfo;
                        break;
                    }
                    // else if (selectedItem is FileInfo fileInfo)   // Must be a FileInfo (do check to avoid null ref warning)
                    // {   // A file has been selected
                    //     return fileInfo;
                    // }
                }
                throw new UnreachableException("Not expected to reach this code!");
            }
        } while (true);
        return pickedFile;

        // Helper method
        static bool DriveExists(string DriveLetter)
        {
            // Remove ' or ' from availableDrives string
            string drives = Regex.Replace(availableDrives, " or ", ",");
            return drives.Contains(DriveLetter);
        }
    }

    /// <summary>
    /// Pick a folder, with the initial directory being %userprofile%
    /// </summary>
    /// <returns></returns>
    public static DirectoryInfo? PickFolder()
    {
        return PickFolder(initialPath);
    }

    /// <summary>
    /// Prompts user to pick a folder, specifying the initial folder to display.
    /// </summary>
    /// <param name="InitialPath"> - the initial folder to display</param>
    /// <returns>A <c>DirectoryInfo</c> representing the folder picked, or null if no folder picked.</returns>
    public static DirectoryInfo? PickFolder(string InitialPath)
    {
        DirectoryInfo_Picker folderPicker = new()
        {
            HelpMessage = "<Enter> to continue if listing paused; <Space> to select current folder;\n" +
        "(n)n to drill down to nn'th folder; .. for parent folder; ? for Help\n\nPlease enter your response: "
        };
        string? userResponse;
        int SelectedFolderIndex;
        bool showHidden = true;
        DirectoryInfo[] foldersArray;
        if (InitialPath == "Downloads") InitialPath = initialPath + @"\Downloads";
        if (!Directory.Exists(InitialPath))
            InitialPath = initialPath;      // Revert to UserProfile folder if Downloads not is std location
        currentFolder = new DirectoryInfo(InitialPath);

        do
        {
            folderPicker.BatchHeading = currentFolder.FullName + (showHidden ? " (showing Hidden *H)" : "");
            foldersArray = currentFolder.EnumerateDirectories("*", enumerationOptions).ToArray<DirectoryInfo>();
            userResponse = folderPicker.PickItem(foldersArray, 40);
            // Evaluate response
            if (userResponse == null) return null; // Return null (indicates no folder was selected)
            if (userResponse == " ") break;         // The current folder was selected
            if (userResponse.ToUpper() == "H")      // Toggle Hidden folders
            {
                enumerationOptions.AttributesToSkip ^= FileAttributes.Hidden;
                showHidden = !showHidden;
                continue;
            }
            if (userResponse == "..")               // Change folder focus to parent folder
            {
                if (currentFolder.Parent == null)
                {
                    bool invalidResponse = true;
                    do
                    {
                        System.Console.Write($"Select an available Drive ({availableDrives}): ");
                        var Response = Console.ReadLine();
                        if (Response == "" || Response == null) continue;
                        var match = Regex.Match(Response, @"([a-zA-Z])($|:|:\\)");
                        if (!match.Success) continue;
                        string firstChar = match.Groups[1].Value.ToUpper();
                        if (!DriveExists(firstChar))
                        {
                            System.Console.WriteLine("Invalid drive specification");
                            continue;
                        }
                        currentFolder = new DirectoryInfo(firstChar + ":\\");
                        invalidResponse = false;
                    } while (invalidResponse);
                }
                else
                    currentFolder = currentFolder.Parent;
            }
            else                                    // Change folder focus to selected folder
            {
                SelectedFolderIndex = int.Parse(userResponse);
                currentFolder = foldersArray[SelectedFolderIndex];
            }
        } while (true);
        return currentFolder;

        // Helper method
        static bool DriveExists(string DriveLetter)
        {
            // Remove ' or ' from availableDrives string
            string drives = Regex.Replace(availableDrives, " or ", ",");
            return drives.Contains(DriveLetter);
        }
    }
}

class FileSystemInfo_Picker : ItemPicker<FileSystemInfo>
{
    protected override string ItemFormatter(FileSystemInfo Item)
    {
        var name = Item.Name;
        if (Item is FileInfo)
            return name;
        else    // Item is DirectoryInfo
        {
            var hiddenTag = (Item.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ? "*H" : "";
            return String.Format($"[{name}] {hiddenTag}");
        }
    }
    protected override bool IsAUserAllowedResponse(string? Response)
    {
        if (Response == null) return true;                  // Null response is valid (i.e. no selection made)
        if (Response.ToUpper() == "H") return true;         // Toggle Hidden folders (h or H)
        if (Response == "..") return true;                  // Instruction to move to parent folder
        return false;                                       // Any other string is invalid
    }
}

class DirectoryInfo_Picker : ItemPicker<DirectoryInfo>
{
    protected override string ItemFormatter(DirectoryInfo Item)
    {
        var name = Item.Name;
        var hiddenTag = (Item.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ? "*H" : "";
        return String.Format($"{name} {hiddenTag}");
    }
    protected override bool IsAUserAllowedResponse(string? Response)
    {
        if (Response == null) return true;                  // Null response is valid (i.e. no selection made)
        if (Response == " ") return true;                   // Pick current folder
        if (Response.ToUpper() == "H") return true;         // Toggle Hidden folders (h or H)
        if (Response == "..") return true;                  // Instruction to move to parent folder
        return false;                                       // Any other string is invalid
    }
}