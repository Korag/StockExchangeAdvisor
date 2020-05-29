using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utility
{
    public static class FileHelper
    {
        public static string ReadFile(string url)
        {
            string fileText = "";

            if (File.Exists(url))
            {
                fileText = File.ReadAllText(url);
            }

            return fileText;
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static int CountFilesInDirectory(string path)
        {
            return Directory.GetFiles(path).Count();
        }

        public static List<string> GetFileNames(string path)
        {
            List<string> pathsToFiles = Directory.GetFiles(path, "*.mst").ToList();

            List<string> filesShortNames = new List<string>();
            pathsToFiles.ForEach(z => filesShortNames.Add(Path.GetFileNameWithoutExtension(z)));

            return filesShortNames;
        }

        public static void SaveJsonFile(string path, string jsonString)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(jsonString);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error creating a json file.");
                throw e;
            }
        }
    }
}
