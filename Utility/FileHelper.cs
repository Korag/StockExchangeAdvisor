using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utility
{
    public static class FileHelper
    {
        public static string ReadFile(string url)
        {
            string fileText = "";

            if (!File.Exists(url))
            {
                fileText = File.ReadAllText(url);
            }

            return fileText;
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static List<string> GetFileNames(string path)
        {
            List<string> pathsToFiles = Directory.GetFiles(path, "*.mst").ToList();

            List<string> filesShortNames = new List<string>();
            pathsToFiles.ForEach(z => filesShortNames.Add(Path.GetFileNameWithoutExtension(z)));

            return filesShortNames;
        }
    }
}
