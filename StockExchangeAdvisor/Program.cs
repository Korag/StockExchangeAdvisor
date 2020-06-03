using System;
using TechnicalIndicators;
using FacadeDesignPattern;
using BuilderDesignPattern.AlgorithmBuilder;
using UtilityAzure;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Signals
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();
            AzureWebServiceHelper aws = new AzureWebServiceHelper();
            //aws.StartVM();

            CoreFacade facade = new CoreFacade(_serviceProvider.GetService<IAlgorithmBuilder>());

            Stopwatch sw = new Stopwatch();
            sw.Start();
           
            //facade.CountSingleIndicatorForSingleCompanyQuotes(new TechnicalIndicatorEMA(), "zywiec");
            facade.CountIndicatorsSetForSingleCompanyQuotes("zywiec");

            //facade.CountSingleIndicatorForAllCompaniesQuotes(new TechnicalIndicatorEMA());
            //facade.CountIndicatorsSetForAllCompaniesQuotes();

            facade.Dispose();
            sw.Stop();

            //aws.StopVM();
            DisposeServices();

            Console.WriteLine("Execution time: " + sw.Elapsed);
            Console.ReadLine();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
          
            //collection.AddSingleton<IAlgorithmBuilder, RabbitMQBuilder>();
            //collection.AddSingleton<IAlgorithmBuilder, WebServicesBuilder>();
            collection.AddSingleton<IAlgorithmBuilder, ActorModelBuilder>();

            _serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
