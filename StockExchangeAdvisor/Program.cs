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
            AzureWebServiceHelper aws = new AzureWebServiceHelper();
            aws.StartVM();


            Facade facade = new Facade(builder);

            Stopwatch sw = new Stopwatch();
            sw.Start();
           
            facade.CountSingleIndicatorForSingleCompanyQuotes(new TechnicalIndicatorEMA(), "zywiec");
            //facade.CountIndicatorsSetForSingleCompanyQuotes("zywiec");

            //facade.CountSingleIndicatorForAllCompaniesQuotes(new TechnicalIndicatorEMA());
            //facade.CountIndicatorsSetForAllCompaniesQuotes();

            facade.Dispose();
            sw.Stop();

            aws.StopVM();
            Console.WriteLine("Execution time: " + sw.Elapsed);
            Console.ReadLine();
        }
    }
}
