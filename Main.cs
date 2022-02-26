using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
namespace DownloadSorterLibrary {

    // Following way to get folders taken from https://stackoverflow.com/questions/10667012/getting-downloads-folder-in-c
    // Answer by Ray https://stackoverflow.com/users/777985/ray

    /// <summary>
    /// Class containing methods to retrieve specific file system paths.
    /// </summary>
    public static class KnownFolders
    {
        private static string[] _knownFolderGuids = new string[]
        {
        "{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts
        "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
        "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
        "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
        "{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
        "{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
        "{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
        "{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
        "{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // SavedGames
        "{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // SavedSearches
        "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
        };

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        public static string GetPath(KnownFolder knownFolder)
        {
            return GetPath(knownFolder, false);
        }

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <param name="defaultUser">Specifies if the paths of the default user (user profile
        ///     template) will be used. This requires administrative rights.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        public static string GetPath(KnownFolder knownFolder, bool defaultUser)
        {
            return GetPath(knownFolder, KnownFolderFlags.DontVerify, defaultUser);
        }

        private static string GetPath(KnownFolder knownFolder, KnownFolderFlags flags,
            bool defaultUser)
        {
            int result = SHGetKnownFolderPath(new Guid(_knownFolderGuids[(int)knownFolder]),
                (uint)flags, new IntPtr(defaultUser ? -1 : 0), out IntPtr outPath);
            if (result >= 0)
            {
                string path = Marshal.PtrToStringUni(outPath);
                Marshal.FreeCoTaskMem(outPath);
                return path;
            }
            else
            {
                throw new ExternalException("Unable to retrieve the known folder path. It may not "
                    + "be available on this system.", result);
            }
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken,
            out IntPtr ppszPath);

        [Flags]
        private enum KnownFolderFlags : uint
        {
            SimpleIDList = 0x00000100,
            NotParentRelative = 0x00000200,
            DefaultPath = 0x00000400,
            Init = 0x00000800,
            NoAlias = 0x00001000,
            DontUnexpand = 0x00002000,
            DontVerify = 0x00004000,
            Create = 0x00008000,
            NoAppcontainerRedirection = 0x00010000,
            AliasOnly = 0x80000000
        }
    }

    /// <summary>
    /// Standard folders registered with the system. These folders are installed with Windows Vista
    /// and later operating systems, and a computer will have only folders appropriate to it
    /// installed.
    /// </summary>
    public enum KnownFolder
    {
        Contacts,
        Desktop,
        Documents,
        Downloads,
        Favorites,
        Links,
        Music,
        Pictures,
        SavedGames,
        SavedSearches,
        Videos
    }

    // End of stackoverflow answered

    public static class FileExtensions
    {
        public static List<string> archiveExt = new List<string>()
            {
                ".rar",
                ".zip",
                ".tar",
                ".iso",
                ".bz2",
                ".gz",
                ".7z",
                ".apk"

            };

        public static List<string> executableExt = new List<string>()
            {
                ".exe",
                ".msi"
            };

        public static List<string> pictureExt = new List<string>()
            {
                ".png",
                ".jpg",
                ".jpeg",
                ".psd",
                ".raw",
                ".webp",
                ".svg"
            };

        public static List<string> videoExt = new List<string>()
            {
                ".mp4",
                ".mov",
                ".webm",
                ".mpv",
                ".flv",
                ".gif",
                ".video",
                ".mp4v",
                ".mpeg4",
                ".movie",
                ".mvp",
                ".ogv",
                ".mpg",
                ".3mm",
                ".m4v",
                ".mp2v",
                ".h264",
                ".mmv",
                ".m2a",
                ".m2v",
                ".mpg4",
                ".mpl",
                ".ogg",
                ".3gp"

            };
        public static List<string> docExt = new List<string>()
        {
            ".pdf",
            ".docx",
            ".epub",
            ".edoc",
            ".pages",
            ".pub",
            ".word"
        };

        public static List<string> codeExt = new List<string>()
        {
            ".cs",
            ".py",
            ".c",
            ".pyc",
            ".html",
            ".js",
            ".css",
            ".asp",
            ".asax",
            ".ascx",
            ".ashx",
            ".asmx",
            ".aspx",
            ".axd",
            ".applescript",
            ".scpt",
            ".asm",
            ".a51",
            ".inc",
            ".nasm",
            ".bat",
            ".cmd",
            ".cake",
            ".cshtml",
            ".csx",
            ".cpp",
            ".c++",
            ".cc",
            ".cp",
            ".cxx",
            ".h",
            ".h++",
            ".hh",
            ".hpp",
            ".hxx",
            ".inc",
            ".inl",
            ".ipp",
            ".tcc",
            ".tpp"
        };
        public static List<List<string>> ExtensionList = new List<List<string>>() { archiveExt, executableExt, pictureExt, videoExt, docExt, codeExt };

        public static List<string> Folder = new List<string>()
        {
            "Zips",
            "Executables",
            "Pics",
            "Videos",
            "Documents",
            "Code"

        };

        public static void CheckAndMove(ExtensionFlags flag, FileInfo file, string path)
        {
            List<string> Extensions = ExtensionList[(int)flag];
            if (Extensions.Contains(file.Extension))
            {
                if (!Directory.Exists($"{path}\\{Folder[(int)flag]}"))
                    Directory.CreateDirectory($"{path}\\{Folder[(int)flag]}");
                string dest = $"{path}\\{Folder[(int)flag]}\\{file.Name}";
                int n = 1;
                while (File.Exists(dest))
                {
                    string NewDest = $"{path}\\{Folder[(int)flag]}\\{file.Name.Replace(file.Extension, String.Empty)}{n}{file.Extension}";
                    if (!File.Exists(NewDest))
                    {
                        dest = NewDest;
                        break;
                    }
                    else
                    {
                        n++;
                    }
                }
                file.MoveTo(dest);
            }
        }

        private static bool CheckAndMove(this FileInfo file, ExtensionFlags flag, string path)
        {
            List<string> Extensions = ExtensionList[(int)flag];
            if (Extensions.Contains(file.Extension))
            {
                if (!Directory.Exists($"{path}\\{Folder[(int)flag]}"))
                    Directory.CreateDirectory($"{path}\\{Folder[(int)flag]}");
                string dest = $"{path}\\{Folder[(int)flag]}\\{file.Name}";
                int n = 1;
                while (File.Exists(dest))
                {
                    string NewDest = $"{path}\\{Folder[(int)flag]}\\{file.Name.Replace(file.Extension, String.Empty)}{n}{file.Extension}";
                    if (!File.Exists(NewDest))
                    {
                        dest = NewDest;
                        break;
                    } else
                    {
                        n++;
                    }
                }
                file.MoveTo(dest);
                return true;
            }
            return false;
        }

        public static void Check()
        {
            if (CurrentTask.path != null)
            {
                DirectoryInfo currentD = new DirectoryInfo(CurrentTask.path);
                foreach (var file in currentD.GetFiles())
                {
                    if (file.CheckAndMove(ExtensionFlags.Archive, CurrentTask.path)) continue;
                    if (file.CheckAndMove(ExtensionFlags.Executable, CurrentTask.path)) continue;
                    if (file.CheckAndMove(ExtensionFlags.Picture, CurrentTask.path)) continue;
                    if (file.CheckAndMove(ExtensionFlags.Video, CurrentTask.path)) continue;
                    if (file.CheckAndMove(ExtensionFlags.Documents, CurrentTask.path)) continue;
                    file.CheckAndMove(ExtensionFlags.Code, CurrentTask.path);
                }
            }
        }
    }

    public enum ExtensionFlags
    {
        Archive,
        Executable,
        Picture,
        Video,
        Documents,
        Code
    }

    public class CurrentTask
    {
        private static CancellationTokenSource src = new CancellationTokenSource();
        private static CancellationToken token = src.Token;
        public static string path = KnownFolders.GetPath(KnownFolder.Downloads);

        public CurrentTask()
        {
            Action action = new Action(FileExtensions.Check);
            Task.Run(() => Loop(action, token, TimeSpan.FromSeconds(10)));
        }

        public async Task Loop(Action action, CancellationToken token, TimeSpan delay)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;
                action();
                await Task.Delay(delay, token);
            }
        }

        public void Cancel()
        {
            src.Cancel();
        }
    }

}