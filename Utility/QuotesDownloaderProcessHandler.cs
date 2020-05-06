using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Utility
{
    public static class QuotesDownloaderProcessHandler
    {
        private static string _processName = "QuotesDownloader";
        private static string _urlToQuotesDownloaderProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\QuotesDownloader\\bin\\Release\\netcoreapp3.1\\{_processName}.exe"));

        public static void RunQuotesDownloaderProcess()
        {
            if (Process.GetProcessesByName(_processName).Count() == 0)
            {
                Process.Start(_urlToQuotesDownloaderProject);
            }
        }

        public static void KillQuotesDownloaderProcess()
        {
            try
            {
                Process quotesDownloaderProcess = Process.GetProcessesByName(_processName).FirstOrDefault();
                quotesDownloaderProcess.Kill();
            }
            catch (Exception)
            {
                Console.WriteLine("QuotesDownloader process not found");
            }
        }
    }
}
