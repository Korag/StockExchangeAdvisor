using Models;
using System;
using System.Collections.Generic;
using System.IO;
using TechnicalIndicators;
using StrategyDesignPattern;

namespace Signals
{
    class Program
    {
        public static string QUOTES_SAVE_PATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\QuotesDownloader\\DownloadedQuotes\\"));

        static void Main(string[] args)
        {
            var zywiecQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(QUOTES_SAVE_PATH + "zywiec.mst");

            var EAM = new ExponentialMovingAverage();
            var parameters = new Parameters
            {
                CalculatedIndicatorFirstDaysInterval = 10,
                CalculatedIndicatorSecondDaysInterval = 5,

                Period = 10
            };

            //var EMA2 = new TechnicalIndicatorEMA();

            //var EAMSignal = EAM.GetSignals(zywiecQuotes, parameters);
            //var EMA2Signal = EMA2.GetSignals(zywiecQuotes, parameters);

            CalculateTechnicalIndicatorContext calculateIndicator = CalculateTechnicalIndicatorContext.GetInstance(new RabbitMQStrategy());
            //calculateIndicator.CalculateSingleIndicator(zywiecQuotes, parameters, new TechnicalIndicatorEMA());
            List<Signal> obtainedSignals = calculateIndicator.ReceiveSignalsFromSingleCalculatedIndicator();

            Console.WriteLine("SIGNALS");
            foreach (var signal in obtainedSignals)
            {
                Console.WriteLine(signal.Date + "/t" + signal.Value);
            }

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
