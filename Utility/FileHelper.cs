using System;
using System.Collections.Generic;
using System.IO;
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
    }
}
