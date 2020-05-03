using BuilderDesignPattern.AlgorithmBuilder;
using Models;
using StrategyDesignPattern;
using System.Collections.Generic;
using TechnicalIndicators;
using Utility;

namespace FacadeDesignPattern
{
    public class RabbitMQFacade
    {
        private AlgorithmManufacturer _algorithmManufacter { get; set; }
        private IAlgorithmBuilder _algorithmBuilder { get; set; }
        private CalculateTechnicalIndicatorContext _calculateContext {get; set;}

        public RabbitMQFacade()
        {
            _algorithmBuilder = new RabbitMQBuilder();
            _algorithmManufacter.Construct(_algorithmBuilder);
            _calculateContext = _algorithmBuilder.StrategyContext;

            AutoMapperHelper.GetInstance();
        }

        public void CountSingleIndicatorForSingleCompanyQuotes(TechnicalIndicator technicalIndicator, string nameOfCompany)
        {
            QuotesDownloaderProcessHandler.RunQuotesDownloaderProcess();

            var zywiecQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(nameOfCompany);

            var parameters = new Parameters
            {
                CalculatedIndicatorFirstDaysInterval = 10,
                CalculatedIndicatorSecondDaysInterval = 5,
                Period = 10
            };

            _calculateContext.CalculateSingleIndicator(zywiecQuotes, parameters, technicalIndicator);
            List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();
       
            List<QuoteWithSignal> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuoteListToQuoteWithSignalList(zywiecQuotes);
            obtainedSignalsWithQuotes = AutoMapperHelper.MapSignalListToQuoteWithSignalList(obtainedSignals, obtainedSignalsWithQuotes);

            Utility.CsvHelper.SaveCompanySignalsToCsvFile(obtainedSignalsWithQuotes, nameOfCompany);
            QuotesDownloaderProcessHandler.KillQuotesDownloaderProcess();
        }

        public void CountSingleIndicatorForAllCompaniesQuotes(TechnicalIndicator technicalIndicator)
        {

        }

        public void CountIndicatorsSetForSingleCompanyQuotes(string nameOfCompany)
        {

        }

        public void CountIndicatorsSetForAllCompaniesQuotes()
        {

        }
    }
}
