using StockExchangeAdvisor.TechnicalIndicators;
using StockExchangeAdvisor.Utility;
using System;
using System.IO;
using System.Reflection;

namespace StockExchangeAdvisor
{
    class Program
    {
        public const string QUOTES_SAVE_PATH = @"C:\Users\user\Documents\Visual Studio 2019\Projects\StockExchangeAdvisor\StockExchangeAdvisor\DownloadedQuotes\UnpackedQuotes\";
        public const string QUOTES_ZIP_PATH = @"C:\Users\user\Documents\Visual Studio 2019\Projects\StockExchangeAdvisor\StockExchangeAdvisor\DownloadedQuotes\mstall.zip";
        public const string QUOTES_FILENAME= "mstall.zip";

        static void Main(string[] args)
        {
            Utility.CsvHelper csvH = new Utility.CsvHelper();
            Utility.ZipHelper zipH = new Utility.ZipHelper();

            //zipH.ExtractZipDirectory(QUOTES_ZIP_PATH, QUOTES_SAVE_PATH);
            var zywiecQuotes = csvH.ReadSingleCsvFileWithQuotes(QUOTES_SAVE_PATH + "zywiec.csv");

            var EAM = new ExponentialMovingAverage();
            var parameters = new Parameters
            {
                CalculatedIndicatorFirstDaysInterval = 10,
                CalculatedIndicatorSecondDaysInterval = 5
            };

            var EAMSignal = EAM.GetSignals(zywiecQuotes, parameters);

            //Console.WriteLine(-1);
            //for (int i = 0; i < EAMSignal.Count; i++)
            //{
            //    if (EAMSignal[i].Value == -1)
            //    {
            //        Console.WriteLine(EAMSignal[i].Date);
            //    }
            //}


            Console.ReadLine();
        }
    }
}
