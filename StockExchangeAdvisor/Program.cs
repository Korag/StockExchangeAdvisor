using System;
using TechnicalIndicators;
using FacadeDesignPattern;
using BuilderDesignPattern.AlgorithmBuilder;
using UtilityAzure;
using System.Diagnostics;

namespace Signals
{
    class Program
    {
        //public static string QUOTES_SAVE_PATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\QuotesDownloader\\DownloadedQuotes\\"));
        //public static IAlgorithmBuilder builder = new RabbitMQBuilder();
        public static IAlgorithmBuilder builder = new WebServicesBuilder();
        //public static IAlgorithmBuilder builder = new ActorModelBuilder();

        static void Main(string[] args)
        {
            #region LegacyCode
            //var zywiecQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes("zywiec");

            //var EAM = new ExponentialMovingAverage();
            //var parameters = new Parameters
            //{
            //    CalculatedIndicatorFirstDaysInterval = 10,
            //    CalculatedIndicatorSecondDaysInterval = 5,

            //    Period = 10
            //};

            //CalculateTechnicalIndicatorContext calculateIndicator = CalculateTechnicalIndicatorContext.GetInstance(new RabbitMQStrategy());
            //calculateIndicator.CalculateSingleIndicator(zywiecQuotes, parameters, new TechnicalIndicatorEMA());
            //List<Signal> obtainedSignals = calculateIndicator.ReceiveSignalsFromSingleCalculatedIndicator();
            #endregion

            AzureWebServiceHelper.StartVM().Wait(-1);
            //AzureWebServiceHelper.StopVM();

            Facade facade = new Facade(builder);

            Stopwatch sw = new Stopwatch();
            sw.Start();
           
            facade.CountSingleIndicatorForSingleCompanyQuotes(new TechnicalIndicatorEMA(), "zywiec");
            //facade.CountIndicatorsSetForSingleCompanyQuotes("zywiec");

            //facade.CountSingleIndicatorForAllCompaniesQuotes(new TechnicalIndicatorEMA());
            //facade.CountIndicatorsSetForAllCompaniesQuotes();

            facade.Dispose();
            sw.Stop();

            Console.WriteLine("Execution time: " + sw.Elapsed);
            Console.ReadLine();

        }
    }
}
