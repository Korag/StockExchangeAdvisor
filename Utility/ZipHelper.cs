using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;

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
            catch (Exception)
            {
                throw;
            }
        }

        public static void SaveZipArchiveFromByteArray(byte[] content, string url)
        {
            File.WriteAllBytes(url, content);
        }
    }
}
