using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Utility
{
    public static class ProcessHandler
    {
        private static string _quotesDownloaderProcessName = "QuotesDownloader";
        private static string _urlToQuotesDownloaderProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\QuotesDownloader\\bin\\Release\\netcoreapp3.1\\{_quotesDownloaderProcessName}.exe"));

        private static string _rabbitMQConsumerEMAProcessName = "RabbitMQConsumerEMA";
        private static string _urlToRabbitMQConsumerEMAProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\QuotesDownloader\\bin\\Release\\netcoreapp3.1\\{_rabbitMQConsumerEMAProcessName}.exe"));
        private static string _rabbitMQConsumerROCProcessName = "RabbitMQConsumerROC";
        private static string _urlToRabbitMQConsumerROCProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\QuotesDownloader\\bin\\Release\\netcoreapp3.1\\{_rabbitMQConsumerROCProcessName}.exe"));

        public static void RunProcess(string processName, string exeUrl)
        {
            if (Process.GetProcessesByName(processName).Count() == 0)
            {
                Process.Start(exeUrl);
            }
        }

        public static void KillProcess(string processName)
        {
            try
            {
                Process process = Process.GetProcessesByName(processName).FirstOrDefault();
                process.Kill();
            }
            catch (Exception e)
            {
                Console.WriteLine("Process not found");
            }
        }

        public static void RunQuotesDownloaderProcess()
        {
            RunProcess(_quotesDownloaderProcessName, _urlToQuotesDownloaderProject);
        }

        public static void KillQuotesDownloaderProcess()
        {
            KillProcess(_quotesDownloaderProcessName);
        }

        public static void RunRabbitMQConsumersProcesses()
        {
            RunProcess(_rabbitMQConsumerEMAProcessName, _urlToRabbitMQConsumerEMAProject);
            RunProcess(_rabbitMQConsumerROCProcessName, _urlToRabbitMQConsumerROCProject);
        }

        public static void KillRabbitMQConsumersProcesses()
        {
            KillProcess(_rabbitMQConsumerEMAProcessName);
            KillProcess(_rabbitMQConsumerROCProcessName);
        }
    }
}
