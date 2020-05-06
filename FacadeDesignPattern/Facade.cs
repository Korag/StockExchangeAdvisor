using BuilderDesignPattern.AlgorithmBuilder;
using DecoratorDesignPattern;
using Models;
using StateAndDecoratorDesignPattern;
using StrategyDesignPattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechnicalIndicators;
using Utility;

namespace FacadeDesignPattern
{
    public class Facade
    {
        private AlgorithmManufacturer _algorithmManufacter { get; set; }
        private IAlgorithmBuilder _algorithmBuilder { get; set; }
        private CalculateTechnicalIndicatorContext _calculateContext {get; set;}

        private Parameters _parameters { get; set; }
        private List<TechnicalIndicator> _indicators { get; set; }

        public string PathToUnpackedQuotesDirectory { get; set; }

        //public RabbitMQFacade()
        //{
        //    _algorithmBuilder = new RabbitMQBuilder();
        //    _algorithmManufacter = new AlgorithmManufacturer();

        //    _algorithmManufacter.Construct(_algorithmBuilder);
        //    _calculateContext = _algorithmBuilder.StrategyContext;

        //    PathToUnpackedQuotesDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\QuotesDownloader\\DownloadedQuotes\\"));

        //    _parameters = new Parameters
        //    {
        //        CalculatedIndicatorFirstDaysInterval = 10,
        //        CalculatedIndicatorSecondDaysInterval = 5,
        //        Period = 10
        //    };

        //    InitializeIndicatorsList();
        //    AutoMapperHelper.GetInstance();
        //}

        public Facade(IAlgorithmBuilder algorithmBuilder)
        {
            _algorithmBuilder = algorithmBuilder;
            _algorithmManufacter = new AlgorithmManufacturer();

            _algorithmManufacter.Construct(_algorithmBuilder);
            _calculateContext = _algorithmBuilder.StrategyContext;

            _parameters = new Parameters
            {
                CalculatedIndicatorFirstDaysInterval = 10,
                CalculatedIndicatorSecondDaysInterval = 5,
                Period = 10
            };

            InitializeIndicatorsList();
            AutoMapperHelper.GetInstance();
        }

        private void InitializeIndicatorsList()
        {
            var abstractClass = typeof(TechnicalIndicator);

            var derivedClass = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(z => z.GetTypes())
                .Where(z => abstractClass.IsAssignableFrom(z)
                && !z.IsInterface && !z.IsAbstract)
                .ToList();

            _indicators = new List<TechnicalIndicator>();

            foreach (var type in derivedClass)
            {
                TechnicalIndicator indicator = (TechnicalIndicator)Activator.CreateInstance(type);
                _indicators.Add(indicator);
            }
        }

        public void CountSingleIndicatorForSingleCompanyQuotes(TechnicalIndicator technicalIndicator, string nameOfCompany)
        {
            QuotesDownloaderProcessHandler.RunQuotesDownloaderProcess();

            var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(nameOfCompany);

            _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
            List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();
            List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

            //TODO:
            //chain of responsibility z ustawianiem State -> SignalValue

            //To AutoMapper:
            DecoratorComponent abc = new DecoratorConcreteComponent
            {
                Close = obtainedSignalsWithQuotes[0].Close
            };

            var aaaa = abc.CalculateConst();
            abc = new CommissionDecorator(abc);
            var aaa = abc.CalculateConst();

            abc = new CommissionDecorator(abc);
            abc.CalculateConst();

            obtainedSignalsWithQuotes[0].FinalPrice = abc.CalculateConst();

            //TODO:
            //decorators, które mają wspólny interfejs (lub abstract)

            //TODO:
            //3. deep clone and save to json obiektu SignalModelContext lub któregoś z decoratora

            //TODO:
            //4. save to file

            //List<QuoteWithSignal> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuoteListToQuoteWithSignalList(companyQuotes);
            //obtainedSignalsWithQuotes = AutoMapperHelper.MapSignalListToQuoteWithSignalList(obtainedSignals, obtainedSignalsWithQuotes);
            //Utility.CsvHelper.SaveCompanySignalsToCsvFile(obtainedSignalsWithQuotes, nameOfCompany);

            QuotesDownloaderProcessHandler.KillQuotesDownloaderProcess();
        }

        public void CountSingleIndicatorForAllCompaniesQuotes(TechnicalIndicator technicalIndicator)
        {
            QuotesDownloaderProcessHandler.RunQuotesDownloaderProcess();
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);

            foreach (var companyName in namesOfCompanies)
            {
                var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(companyName);
                
                _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
                List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();
                List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);
                
                //decorators
                //chain of responsibility
                //3. deep clone and save to json
                //4. save to file
                //5. save to divided files by state

                //Utility.CsvHelper.SaveCompanySignalsToCsvFile(obtainedSignalsWithQuotes, companyName);
            }

            QuotesDownloaderProcessHandler.KillQuotesDownloaderProcess();
        }

        public void CountIndicatorsSetForSingleCompanyQuotes(string nameOfCompany)
        {
            QuotesDownloaderProcessHandler.RunQuotesDownloaderProcess();

            var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(nameOfCompany);

            foreach (var indicator in _indicators)
            {
              _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, indicator);
            }

            List<List<Signal>> obtainedSignals = _calculateContext.ReceiveSignalsFromCalculatedIndicators(_indicators.Count());
            List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);
           
            //TODO:
            //1. decorators
            //3. deep clone and save to json
            //4. chain of responsibility and count FinalSignal
            //4. save to file
            //5. save to divided files by state
            //6. deep clone and save to json

            QuotesDownloaderProcessHandler.KillQuotesDownloaderProcess();
        }

        public void CountIndicatorsSetForAllCompaniesQuotes()
        {
            QuotesDownloaderProcessHandler.RunQuotesDownloaderProcess();
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);

            foreach (var companyName in namesOfCompanies)
            {
                var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(companyName);

                foreach (var indicator in _indicators)
                {
                    _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, indicator);
                }

                List<List<Signal>> obtainedSignals = _calculateContext.ReceiveSignalsFromCalculatedIndicators(_indicators.Count());
                List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

                //TODO:
                //1. decorators
                //3. deep clone and save to json
                //4. chain of responsibility and count FinalSignal
                //4. save to file
                //5. save to divided files by state
                //6. deep clone and save to json
            }

            QuotesDownloaderProcessHandler.KillQuotesDownloaderProcess();
        }
    }
}
