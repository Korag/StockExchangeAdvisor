using System;
using System.IO;
using System.IO.Compression;

namespace Utility
{
    public static class ZipHelper
    {
        public static void ExtractZipDirectory(string zipPath, string extractPath)
        {
            try
            {
                using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                {
                    DirectoryInfo di = new DirectoryInfo(extractPath);
                    
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }

                    archive.ExtractToDirectory(extractPath);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error during content extraction of .zip file.");
                throw e;
            }
        }

        public static void SaveZipArchiveFromByteArray(byte[] content, string url)
        {
            try
            {
            File.WriteAllBytes(url, content);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error during saving .zip file.");
                throw e;
            }
        }
    }
}
