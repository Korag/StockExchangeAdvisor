using BuilderDesignPattern.AlgorithmBuilder;
using ChainOfResponsibilityDesignPattern;
using DecoratorDesignPattern;
using Models;
using PrototypeDesignPattern;
using StateAndDecoratorDesignPattern;
using StrategyDesignPattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechnicalIndicators;
using Utility;

namespace FacadeDesignPattern
{
    public class CoreFacade
    {
        private AlgorithmManufacturer _algorithmManufacter { get; set; }
        private IAlgorithmBuilder _algorithmBuilder { get; set; }
        private CalculateTechnicalIndicatorContext _calculateContext { get; set; }

        private Parameters _parameters { get; set; }
        private List<TechnicalIndicator> _indicators { get; set; }

        public string PathToUnpackedQuotesDirectory { get; set; }

        public CoreFacade(IAlgorithmBuilder algorithmBuilder)
        {
            //GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            //GC.TryStartNoGCRegion(4000000000);

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

            if (_algorithmBuilder.GetType() == typeof(RabbitMQBuilder))
            {
                ProcessHandler.KillRabbitMQConsumersProcesses();
            }

            //GC.EndNoGCRegion();
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

            //foreach (var singleQuotePartialSignals in obtainedSignalsWithQuotes)
            Parallel.ForEach(obtainedSignalsWithQuotes, (singleQuotePartialSignals) =>
            {
                ConcreteChainHandlerElement chain = new ConcreteChainHandlerElement();

                ComputeFinalSignalModel finalSignalModel = new ComputeFinalSignalModel(singleQuotePartialSignals.PartialSignals);
                singleQuotePartialSignals.SetSignalValue(chain.DetermineFinalSignal(finalSignalModel));
            }
            );

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

            //foreach (var quoteWSignals in obtainedSignalsWithQuotes)
            Parallel.ForEach(obtainedSignalsWithQuotes, (quoteWSignals) =>
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

                double formattedFee;
                double formattedFinalPrice;

                double.TryParse(String.Format("{0:0.##}", fee), out formattedFee);
                double.TryParse(String.Format("{0:0.##}", finalPrice), out formattedFinalPrice);

                quoteWSignals.AdditionalFee = formattedFee;
                quoteWSignals.FinalPrice = formattedFinalPrice;
            }
            );

            #region DeepClone

            DeepCloneFacade deepClone = new DeepCloneFacade(obtainedSignalsWithQuotes);

            //Deep Clone using JsonSerialization
            List<SignalModelContext> clonedSignalContextByJsonSerializer = deepClone.DeepCloneUsingJsonSerialization();
           
            //Deep Clone using BinarySerialization
            List<SignalModelContext> clonedSignalContextByBinarySerializer = deepClone.DeepCloneUsingBinarySerialization();

            //Deep Clone using Reflection
            List<SignalModelContext> clonedSignalContextByReflection = deepClone.DeepCloneUsingReflection();

            #endregion

            #region SavingResults

            FileHandlerFacade fileHandler = new FileHandlerFacade();

            //Saving JsonFile to PrototypeObjects directory
            fileHandler.SaveJsonFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

            //Saving CsvFile to GeneratedSignals directory
            fileHandler.SaveCsvFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

            #endregion
        }

        public void CountSingleIndicatorForAllCompaniesQuotes(TechnicalIndicator technicalIndicator)
        {
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);

            FileHandlerFacade fileHandler = new FileHandlerFacade();
            List<List<Quote>> companiesQuotes = fileHandler.ReadMultipleCsvFiles(namesOfCompanies);

            int iterationNumber = 0;

            // foreach (var companyName in namesOfCompanies)
            foreach (var companyQuotes in companiesQuotes)
            {
                string nameOfCompany = namesOfCompanies[iterationNumber];

                _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
                List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();
                List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

                //foreach (var singleQuotePartialSignals in obtainedSignalsWithQuotes)
                Parallel.ForEach(obtainedSignalsWithQuotes, (singleQuotePartialSignals) =>
                {
                    ConcreteChainHandlerElement chain = new ConcreteChainHandlerElement();

                    ComputeFinalSignalModel finalSignalModel = new ComputeFinalSignalModel(singleQuotePartialSignals.PartialSignals);
                    singleQuotePartialSignals.SetSignalValue(chain.DetermineFinalSignal(finalSignalModel));
                }
                );

                //foreach (var quoteWSignals in obtainedSignalsWithQuotes)
                Parallel.ForEach(obtainedSignalsWithQuotes, (quoteWSignals) =>
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

                    double formattedFee;
                    double formattedFinalPrice;

                    double.TryParse(String.Format("{0:0.##}", fee), out formattedFee);
                    double.TryParse(String.Format("{0:0.##}", finalPrice), out formattedFinalPrice);

                    quoteWSignals.AdditionalFee = formattedFee;
                    quoteWSignals.FinalPrice = formattedFinalPrice;
                }
                );

                #region DeepClone

                DeepCloneFacade deepClone = new DeepCloneFacade(obtainedSignalsWithQuotes);

                //Deep Clone using JsonSerialization
                List<SignalModelContext> clonedSignalContextByJsonSerializer = deepClone.DeepCloneUsingJsonSerialization();

                //Deep Clone using BinarySerialization
                List<SignalModelContext> clonedSignalContextByBinarySerializer = deepClone.DeepCloneUsingBinarySerialization();

                //Deep Clone using Reflection
                List<SignalModelContext> clonedSignalContextByReflection = deepClone.DeepCloneUsingReflection();

                #endregion

                #region SavingResults

                //Saving JsonFile to PrototypeObjects directory
                fileHandler.SaveJsonFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

                //Saving CsvFile to GeneratedSignals directory
                fileHandler.SaveCsvFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

                #endregion

                iterationNumber++;
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

            //foreach (var singleQuotePartialSignals in obtainedSignalsWithQuotes)
            Parallel.ForEach(obtainedSignalsWithQuotes, (singleQuotePartialSignals) =>
            {
                ConcreteChainHandlerElement chain = new ConcreteChainHandlerElement();

                ComputeFinalSignalModel finalSignalModel = new ComputeFinalSignalModel(singleQuotePartialSignals.PartialSignals);
                singleQuotePartialSignals.SetSignalValue(chain.DetermineFinalSignal(finalSignalModel));
            }
            );

            //foreach (var quoteWSignals in obtainedSignalsWithQuotes)
            Parallel.ForEach(obtainedSignalsWithQuotes, (quoteWSignals) =>
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

                double formattedFee;
                double formattedFinalPrice;

                double.TryParse(String.Format("{0:0.##}", fee), out formattedFee);
                double.TryParse(String.Format("{0:0.##}", finalPrice), out formattedFinalPrice);

                quoteWSignals.AdditionalFee = formattedFee;
                quoteWSignals.FinalPrice = formattedFinalPrice;
            }
            );

            #region DeepClone

            DeepCloneFacade deepClone = new DeepCloneFacade(obtainedSignalsWithQuotes);

            //Deep Clone using JsonSerialization
            List<SignalModelContext> clonedSignalContextByJsonSerializer = deepClone.DeepCloneUsingJsonSerialization();

            //Deep Clone using BinarySerialization
            List<SignalModelContext> clonedSignalContextByBinarySerializer = deepClone.DeepCloneUsingBinarySerialization();

            //Deep Clone using Reflection
            List<SignalModelContext> clonedSignalContextByReflection = deepClone.DeepCloneUsingReflection();

            #endregion

            #region SavingResults

            FileHandlerFacade fileHandler = new FileHandlerFacade();

            //Saving JsonFile to PrototypeObjects directory
            fileHandler.SaveJsonFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

            //Saving CsvFile to GeneratedSignals directory
            fileHandler.SaveCsvFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

            #endregion
        }

        public void CountIndicatorsSetForAllCompaniesQuotes()
        {
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);

            FileHandlerFacade fileHandler = new FileHandlerFacade();
            List<List<Quote>> companiesQuotes = fileHandler.ReadMultipleCsvFiles(namesOfCompanies);

            int iterationNumber = 0;

            // foreach (var companyName in namesOfCompanies)
            foreach (var companyQuotes in companiesQuotes)
            {
                string nameOfCompany = namesOfCompanies[iterationNumber];

                foreach (var indicator in _indicators)
                {
                    _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, indicator);
                }

                List<List<Signal>> obtainedSignals = _calculateContext.ReceiveSignalsFromCalculatedIndicators(_indicators.Count());
                List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

                //foreach (var singleQuotePartialSignals in obtainedSignalsWithQuotes)
                Parallel.ForEach(obtainedSignalsWithQuotes, (singleQuotePartialSignals) =>
                {
                    ConcreteChainHandlerElement chain = new ConcreteChainHandlerElement();

                    ComputeFinalSignalModel finalSignalModel = new ComputeFinalSignalModel(singleQuotePartialSignals.PartialSignals);
                    singleQuotePartialSignals.SetSignalValue(chain.DetermineFinalSignal(finalSignalModel));
                }
                );

                //foreach (var quoteWSignals in obtainedSignalsWithQuotes)
                Parallel.ForEach(obtainedSignalsWithQuotes, (quoteWSignals) =>
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

                    double formattedFee;
                    double formattedFinalPrice;

                    double.TryParse(String.Format("{0:0.##}", fee), out formattedFee);
                    double.TryParse(String.Format("{0:0.##}", finalPrice), out formattedFinalPrice);

                    quoteWSignals.AdditionalFee = formattedFee;
                    quoteWSignals.FinalPrice = formattedFinalPrice;
                }
                );

                #region DeepClone

                DeepCloneFacade deepClone = new DeepCloneFacade(obtainedSignalsWithQuotes);

                //Deep Clone using JsonSerialization
                List<SignalModelContext> clonedSignalContextByJsonSerializer = deepClone.DeepCloneUsingJsonSerialization();

                //Deep Clone using BinarySerialization
                List<SignalModelContext> clonedSignalContextByBinarySerializer = deepClone.DeepCloneUsingBinarySerialization();

                //Deep Clone using Reflection
                List<SignalModelContext> clonedSignalContextByReflection = deepClone.DeepCloneUsingReflection();

                #endregion

                #region SavingResults

                //Saving JsonFile to PrototypeObjects directory
                fileHandler.SaveJsonFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

                //Saving CsvFile to GeneratedSignals directory
                fileHandler.SaveCsvFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

                #endregion

                iterationNumber++;
            }
        }
    }
}
