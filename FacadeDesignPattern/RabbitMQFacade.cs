using BuilderDesignPattern.AlgorithmBuilder;
using Models;
using StrategyDesignPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using TechnicalIndicators;
using Utility;

namespace FacadeDesignPattern
{
    public class RabbitMQFacade
    {
        private AlgorithmManufacturer _algorithmManufacter { get; set; }
        private IAlgorithmBuilder _algorithmBuilder { get; set; }
        private CalculateTechnicalIndicatorContext _calculateContext {get; set;}

        private Parameters _parameters { get; set; }
        private List<TechnicalIndicator> _indicators { get; set; }

        public RabbitMQFacade()
        {
            _algorithmBuilder = new RabbitMQBuilder();
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

        public RabbitMQFacade(IAlgorithmBuilder algorithmBuilder)
        {
            _algorithmBuilder = algorithmBuilder;
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
                TechnicalIndicator indicator = (TechnicalIndicatorEMA)Activator.CreateInstance(type);
                _indicators.Add(indicator);
            }
        }

        public void CountSingleIndicatorForSingleCompanyQuotes(TechnicalIndicator technicalIndicator, string nameOfCompany)
        {
            QuotesDownloaderProcessHandler.RunQuotesDownloaderProcess();

            var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(nameOfCompany);

            _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
            List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();

            //TODO:
            //1. decorators
            //2. consolidation of Signals to SignalModelContext
            //3. deep clone and save to json
            //4. save to file
            //5. save to divided files by state

            //Skipped:
            //1. chain of responsibility

            List<QuoteWithSignal> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuoteListToQuoteWithSignalList(companyQuotes);
            obtainedSignalsWithQuotes = AutoMapperHelper.MapSignalListToQuoteWithSignalList(obtainedSignals, obtainedSignalsWithQuotes);
            Utility.CsvHelper.SaveCompanySignalsToCsvFile(obtainedSignalsWithQuotes, nameOfCompany);
      
            QuotesDownloaderProcessHandler.KillQuotesDownloaderProcess();
        }

        public void CountSingleIndicatorForAllCompaniesQuotes(TechnicalIndicator technicalIndicator)
        {
            QuotesDownloaderProcessHandler.RunQuotesDownloaderProcess();

            //TODO:
            //1. get all company names from folder with downloaded quotes

            List<string> namesOfCompanies = new List<string>();

            foreach (var companyName in namesOfCompanies)
            {
                var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(companyName);
                
                _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
                List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();

                //TODO:
                //1. decorators
                //2. consolidation of Signals to SignalModelContext
                //3. deep clone and save to json
                //4. save to file
                //5. save to divided files by state

                List<QuoteWithSignal> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuoteListToQuoteWithSignalList(companyQuotes);
                obtainedSignalsWithQuotes = AutoMapperHelper.MapSignalListToQuoteWithSignalList(obtainedSignals, obtainedSignalsWithQuotes);
                Utility.CsvHelper.SaveCompanySignalsToCsvFile(obtainedSignalsWithQuotes, companyName);
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

            //TODO:
            //1. decorators
            //2. consolidation of Signals to SignalModelContext
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

            //TODO:
            //1. get all company names from folder with downloaded quotes

            List<string> namesOfCompanies = new List<string>();

            foreach (var companyName in namesOfCompanies)
            {
                var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(companyName);

                foreach (var indicator in _indicators)
                {
                    _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, indicator);
                }

                List<List<Signal>> obtainedSignals = _calculateContext.ReceiveSignalsFromCalculatedIndicators(_indicators.Count());

                //TODO:
                //1. decorators
                //2. consolidation of Signals to SignalModelContext
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
