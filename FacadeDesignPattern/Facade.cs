using BuilderDesignPattern.AlgorithmBuilder;
using DecoratorDesignPattern;
using Models;
using PrototypeDesignPattern;
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
        private CalculateTechnicalIndicatorContext _calculateContext { get; set; }

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

            ProcessHandler.RunQuotesDownloaderProcess();

            InitializeIndicatorsList();
            AutoMapperHelper.GetInstance();
        }

        public void Dispose()
        {
            ProcessHandler.KillQuotesDownloaderProcess();
            ProcessHandler.KillRabbitMQConsumersProcesses();
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
            var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(nameOfCompany);

            _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
            List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();
            List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

            //TODO:
            //1. chain of responsibility z ustawianiem State -> SignalValue na podstawie PartialSignals

            #region DecoratorTests

            //obtainedSignalsWithQuotes[0].CurrentState.SignalValue = 1;

            //DecoratorComponent abc = new DecoratorConcreteComponent
            //{
            //    Close = obtainedSignalsWithQuotes[0].Close,
            //    CurrentState = obtainedSignalsWithQuotes[0].CurrentState,
            //};

            //var costStart = abc.CalculateCost();
            //abc = new CommissionDecorator(abc);
            //var cost1 = abc.CalculateCost();
            //var fee1 = abc.CalculateAdditionalFee();

            //abc = new CommissionDecorator(abc);
            //var cost2 = abc.CalculateCost();
            //var fee2 = abc.CalculateAdditionalFee();

            //obtainedSignalsWithQuotes[0].FinalPrice = abc.CalculateCost();

            #endregion

            foreach (var quoteWSignals in obtainedSignalsWithQuotes)
            {
                double fee = 0;
                double finalPrice = 0;

                DecoratorComponent decorator = new DecoratorConcreteComponent();
                decorator = AutoMapperHelper.MapQuotesAndSignalsToDecoratorObject(quoteWSignals);

                finalPrice = decorator.CalculateCost();

                //Dekorator prowizji
                decorator = new CommissionDecorator(decorator);
                finalPrice = decorator.CalculateCost();
                fee += decorator.CalculateAdditionalFee();

                //Dekorator podatku
                decorator = new TaxDecorator(decorator);
                finalPrice = decorator.CalculateCost();
                fee += decorator.CalculateAdditionalFee();

                //Dekorator konwersji PLN to USD
                //decorator = new ConversionFromPLNtoUSDDecorator(decorator);
                //finalPrice = decorator.CalculateCost();
                //fee += decorator.CalculateAdditionalFee();

                //Dekorator konwersji PLN to EUR
                //decorator = new ConversionFromPLNtoEURDecorator(decorator);
                //finalPrice = decorator.CalculateCost();
                //fee += decorator.CalculateAdditionalFee();

                quoteWSignals.AdditionalFee = fee;
                quoteWSignals.FinalPrice = finalPrice;
            }

            //Deep Clone using JsonSerialization
            List<SignalModelContext> clonedSignalContextByJsonSerializer = new List<SignalModelContext>();

            foreach (var signal in obtainedSignalsWithQuotes)
            {
                SignalModelContext singleClonedSignalContext = signal.Clone() as SignalModelContext;
                clonedSignalContextByJsonSerializer.Add(singleClonedSignalContext);
            }

            //Deep Clone using BinarySerialization
            obtainedSignalsWithQuotes.ForEach(z => z.JsonSerialization = false);

            List<SignalModelContext> clonedSignalContextByBinarySerializer = new List<SignalModelContext>();

            foreach (var signal in obtainedSignalsWithQuotes)
            {
                SignalModelContext singleClonedSignalContext = signal.Clone() as SignalModelContext;
                clonedSignalContextByBinarySerializer.Add(singleClonedSignalContext);
            }

            //Deep Clone using Reflection
            List<SignalModelContext> clonedSignalContextByReflection = new List<SignalModelContext>();

            foreach (var signal in obtainedSignalsWithQuotes)
            {
                SignalModelContext singleClonedSignalContext = ReflectionDeepCopy.CloneObject(signal) as SignalModelContext;
                clonedSignalContextByReflection.Add(singleClonedSignalContext);
            }

            //Saving JsonFile to PrototypeObjects directory
            string jsonString = JsonSerializer.SignalModelContextListToJsonString(obtainedSignalsWithQuotes);
            DateTime currentDateTime = DateTime.Now;
            string dateTimeFormat = "ddMMyyyy-HHmm";
            string fileName = nameOfCompany + "_" + currentDateTime.ToString(dateTimeFormat) + "-" +
                                Convert.ToString((int)currentDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) + "_prototypeObject";
            
            FileHelper.SaveJsonFile(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\StockExchangeAdvisor\\PrototypeObjects\\{fileName}.json")), jsonString);

            //TODO:
            //3. save to csv file with all proper properties and factor from state pattern 
        }

        public void CountSingleIndicatorForAllCompaniesQuotes(TechnicalIndicator technicalIndicator)
        {
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);

            foreach (var companyName in namesOfCompanies)
            {
                var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(companyName);

                _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
                List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();
                List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

                //1. chain of responsibility

                foreach (var quoteWSignals in obtainedSignalsWithQuotes)
                {
                    double fee = 0;
                    double finalPrice = 0;

                    DecoratorComponent decorator = new DecoratorConcreteComponent();
                    decorator = AutoMapperHelper.MapQuotesAndSignalsToDecoratorObject(quoteWSignals);

                    finalPrice = decorator.CalculateCost();

                    //Dekorator prowizji
                    decorator = new CommissionDecorator(decorator);
                    finalPrice = decorator.CalculateCost();
                    fee += decorator.CalculateAdditionalFee();

                    //Dekorator podatku
                    decorator = new TaxDecorator(decorator);
                    finalPrice = decorator.CalculateCost();
                    fee += decorator.CalculateAdditionalFee();

                    //Dekorator konwersji PLN to USD
                    //decorator = new ConversionFromPLNtoUSDDecorator(decorator);
                    //finalPrice = decorator.CalculateCost();
                    //fee += decorator.CalculateAdditionalFee();

                    //Dekorator konwersji PLN to EUR
                    //decorator = new ConversionFromPLNtoEURDecorator(decorator);
                    //finalPrice = decorator.CalculateCost();
                    //fee += decorator.CalculateAdditionalFee();

                    quoteWSignals.AdditionalFee = fee;
                    quoteWSignals.FinalPrice = finalPrice;
                }

                //2. deep clone and save to json
                //3. save to file
            }
        }

        public void CountIndicatorsSetForSingleCompanyQuotes(string nameOfCompany)
        {
            var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(nameOfCompany);

            foreach (var indicator in _indicators)
            {
                _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, indicator);
            }

            List<List<Signal>> obtainedSignals = _calculateContext.ReceiveSignalsFromCalculatedIndicators(_indicators.Count());
            List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

            //1. chain of responsibility and count FinalSignal

            foreach (var quoteWSignals in obtainedSignalsWithQuotes)
            {
                double fee = 0;
                double finalPrice = 0;

                DecoratorComponent decorator = new DecoratorConcreteComponent();
                decorator = AutoMapperHelper.MapQuotesAndSignalsToDecoratorObject(quoteWSignals);

                finalPrice = decorator.CalculateCost();

                //Dekorator prowizji
                decorator = new CommissionDecorator(decorator);
                finalPrice = decorator.CalculateCost();
                fee += decorator.CalculateAdditionalFee();

                //Dekorator podatku
                decorator = new TaxDecorator(decorator);
                finalPrice = decorator.CalculateCost();
                fee += decorator.CalculateAdditionalFee();

                //Dekorator konwersji PLN to USD
                //decorator = new ConversionFromPLNtoUSDDecorator(decorator);
                //finalPrice = decorator.CalculateCost();
                //fee += decorator.CalculateAdditionalFee();

                //Dekorator konwersji PLN to EUR
                //decorator = new ConversionFromPLNtoEURDecorator(decorator);
                //finalPrice = decorator.CalculateCost();
                //fee += decorator.CalculateAdditionalFee();

                quoteWSignals.AdditionalFee = fee;
                quoteWSignals.FinalPrice = finalPrice;
            }

            //2. deep clone and save to json
            //3. save to file
        }

        public void CountIndicatorsSetForAllCompaniesQuotes()
        {
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

                //1. chain of responsibility and count FinalSignal

                foreach (var quoteWSignals in obtainedSignalsWithQuotes)
                {
                    double fee = 0;
                    double finalPrice = 0;

                    DecoratorComponent decorator = new DecoratorConcreteComponent();
                    decorator = AutoMapperHelper.MapQuotesAndSignalsToDecoratorObject(quoteWSignals);

                    finalPrice = decorator.CalculateCost();

                    //Dekorator prowizji
                    decorator = new CommissionDecorator(decorator);
                    finalPrice = decorator.CalculateCost();
                    fee += decorator.CalculateAdditionalFee();

                    //Dekorator podatku
                    decorator = new TaxDecorator(decorator);
                    finalPrice = decorator.CalculateCost();
                    fee += decorator.CalculateAdditionalFee();

                    //Dekorator konwersji PLN to USD
                    //decorator = new ConversionFromPLNtoUSDDecorator(decorator);
                    //finalPrice = decorator.CalculateCost();
                    //fee += decorator.CalculateAdditionalFee();

                    //Dekorator konwersji PLN to EUR
                    //decorator = new ConversionFromPLNtoEURDecorator(decorator);
                    //finalPrice = decorator.CalculateCost();
                    //fee += decorator.CalculateAdditionalFee();

                    quoteWSignals.AdditionalFee = fee;
                    quoteWSignals.FinalPrice = finalPrice;
                }

                //2. deep clone and save to json

                //3. save to file
            }
        }
    }
}
