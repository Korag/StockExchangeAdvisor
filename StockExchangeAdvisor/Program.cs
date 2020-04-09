using Models;
using System;
using TechnicalIndicators;

namespace Signals
{
    class Program
    {
        public const string QUOTES_SAVE_PATH = @"E:\Projects\Visual Studio 2019\StockExchangeAdvisor\StockExchangeAdvisor\DownloadedQuotes\UnpackedQuotes\";
        public const string QUOTES_ZIP_PATH = @"E:\Projects\Visual Studio 2019\StockExchangeAdvisor\StockExchangeAdvisor\DownloadedQuotes\mstall.zip";
        public const string QUOTES_FILENAME= "mstall.zip";

        static void Main(string[] args)
        {
            //Utility.ZipHelper.ExtractZipDirectory(QUOTES_ZIP_PATH, QUOTES_SAVE_PATH);
            var zywiecQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(QUOTES_SAVE_PATH + "zywiec.csv");

            var EAM = new ExponentialMovingAverage();
            var parameters = new Parameters
            {
                CalculatedIndicatorFirstDaysInterval = 10,
                CalculatedIndicatorSecondDaysInterval = 5,

                Period = 10
            };

            var EMA2 = new TechnicalIndicatorEMA();

            var EAMSignal = EAM.GetSignals(zywiecQuotes, parameters);
            var EMA2Signal = EMA2.GetSignals(zywiecQuotes, parameters);


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
