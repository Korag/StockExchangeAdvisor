using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Utility
{
    public static class ProcessHandler
    {
        #region URL

        private static string _quotesDownloaderProcessName = "QuotesDownloader";
        private static string _urlToQuotesDownloaderProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\QuotesDownloader\\bin\\Release\\netcoreapp3.1\\{_quotesDownloaderProcessName}.exe"));

        private static string _rabbitMQConsumerEMAProcessName = "RabbitMQConsumerEMA";
        private static string _urlToRabbitMQConsumerEMAProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\RabbitMQConsumerEMA\\bin\\Release\\netcoreapp3.1\\{_rabbitMQConsumerEMAProcessName}.exe"));

        private static string _rabbitMQConsumerEMAExtendedProcessName = "RabbitMQConsumerEMAExtended";
        private static string _urlToRabbitMQConsumerEMAExtendedProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\RabbitMQConsumerEMAExtended\\bin\\Release\\netcoreapp3.1\\{_rabbitMQConsumerEMAExtendedProcessName}.exe"));

        private static string _rabbitMQConsumerROCProcessName = "RabbitMQConsumerROC";
        private static string _urlToRabbitMQConsumerROCProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\RabbitMQConsumerROC\\bin\\Release\\netcoreapp3.1\\{_rabbitMQConsumerROCProcessName}.exe"));

        private static string _rabbitMQConsumerROC2ProcessName = "RabbitMQConsumerROC2";
        private static string _urlToRabbitMQConsumerROC2Project = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\RabbitMQConsumerROC2\\bin\\Release\\netcoreapp3.1\\{_rabbitMQConsumerROC2ProcessName}.exe"));

        private static string _rabbitMQConsumerSMAProcessName = "RabbitMQConsumerSMA";
        private static string _urlToRabbitMQConsumerSMAProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\RabbitMQConsumerSMA\\bin\\Release\\netcoreapp3.1\\{_rabbitMQConsumerSMAProcessName}.exe"));

        private static string _rabbitMQConsumerCandleFormationProcessName = "RabbitMQConsumerCandleFormation";
        private static string _urlToRabbitMQConsumerCandleFormationProject = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\RabbitMQConsumerCandleFormation\\bin\\Release\\netcoreapp3.1\\{_rabbitMQConsumerCandleFormationProcessName}.exe"));

        #endregion

        public static void RunProcess(string processName, string exeUrl)
        {
            if (Process.GetProcessesByName(processName).Count() == 0)
            {
                Process.Start(exeUrl);
                Console.WriteLine(processName + " : process opened.");
            }
        }

        public static void KillProcess(string processName)
        {
            try
            {
                Process process = Process.GetProcessesByName(processName).FirstOrDefault();
                process.Kill();
                Console.WriteLine(processName + " : process closed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Process not found.");
                throw e;
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
            RunProcess(_rabbitMQConsumerEMAExtendedProcessName, _urlToRabbitMQConsumerEMAExtendedProject);

            RunProcess(_rabbitMQConsumerROCProcessName, _urlToRabbitMQConsumerROCProject);
            RunProcess(_rabbitMQConsumerROC2ProcessName, _urlToRabbitMQConsumerROC2Project);

            RunProcess(_rabbitMQConsumerSMAProcessName, _urlToRabbitMQConsumerSMAProject);
            RunProcess(_rabbitMQConsumerCandleFormationProcessName, _urlToRabbitMQConsumerCandleFormationProject);
        }

        public static void KillRabbitMQConsumersProcesses()
        {
            KillProcess(_rabbitMQConsumerEMAProcessName);
            KillProcess(_rabbitMQConsumerEMAExtendedProcessName);

            KillProcess(_rabbitMQConsumerROCProcessName); 
            KillProcess(_rabbitMQConsumerROC2ProcessName);

            KillProcess(_rabbitMQConsumerSMAProcessName);
            KillProcess(_rabbitMQConsumerCandleFormationProcessName);
        }
    }
}
