using System;
using System.IO.Compression;

namespace Utility
{
    public static class ZipHelper
    {
        public static void ExtractZipDirectory(string zipPath, string extractPath)
        {
            try
            {
                // check directory if is empty
                // if yes extract
                // if no delete all files

                using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                {
                    archive.ExtractToDirectory(extractPath);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
