using BuilderDesignPattern.AlgorithmBuilder;
using Models;
using StateAndDecoratorDesignPattern;
using StrategyDesignPattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using TechnicalIndicators;
using Utility;

namespace FacadeDesignPattern
{
    public class CoreFacade
    {
        private Object _padlock { get; set; }

        private AlgorithmManufacturer _algorithmManufacter { get; set; }
        private IAlgorithmBuilder _algorithmBuilder { get; set; }
        private CalculateTechnicalIndicatorContext _calculateContext { get; set; }

        private Parameters _parameters { get; set; }
        private List<TechnicalIndicator> _indicators { get; set; }

        private DecoratorFacade _decorator { get; set; }
        private DeepCloneFacade _deepClone { get; set; }
        private FileHandlerFacade _fileHandler { get; set; }
        private ChainOfResponsibilityFacade _chainOfResponsibility { get; set; }

        private bool _gcLowLatency { get; set; }
        private bool _noGCRegion { get; set; }

        public string PathToUnpackedQuotesDirectory { get => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\QuotesDownloader\\DownloadedQuotes\\"));}

        public CoreFacade(IAlgorithmBuilder algorithmBuilder, bool gcLowLatency = false, bool noGCRegion = false)
        {
            _gcLowLatency = gcLowLatency;
            _noGCRegion = noGCRegion;

            if (_gcLowLatency == true)
                GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            if (_noGCRegion == true)
                GC.TryStartNoGCRegion(4000000000);

            _algorithmBuilder = algorithmBuilder;
            _algorithmManufacter = new AlgorithmManufacturer();

            _algorithmManufacter.Construct(_algorithmBuilder);
            _calculateContext = _algorithmBuilder.StrategyContext;

            _parameters = new Parameters
            {
                CalculatedIndicatorFirstDaysInterval = 10,
                CalculatedIndicatorSecondDaysInterval = 5,
                Period = 10,
                BuyTrigger = 50,
                SellTrigger = 10,
                NQuotesBackwards = 3
            };

            _decorator = new DecoratorFacade();
            _deepClone = new DeepCloneFacade();
            _fileHandler = new FileHandlerFacade();
            _chainOfResponsibility = new ChainOfResponsibilityFacade();

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

            if (_noGCRegion == true)
                GC.EndNoGCRegion();
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

            #region ChainOfResponsibility

            _chainOfResponsibility.ExecuteChainOfPartialSignalToDeterminFinalSignal(ref obtainedSignalsWithQuotes);

            #endregion

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

            ////foreach (var quoteWSignals in obtainedSignalsWithQuotes)
            //Parallel.ForEach(obtainedSignalsWithQuotes, (quoteWSignals) =>
            //{
            //    double fee = 0;
            //    double finalPrice = 0;

            //    DecoratorComponent decorator = new DecoratorConcreteComponent();
            //    decorator = AutoMapperHelper.MapQuotesAndSignalsToDecoratorObject(quoteWSignals);

            //    finalPrice = decorator.CalculateCost();

            //    //Dekorator prowizji
            //    decorator = new CommissionDecorator(decorator);
            //    finalPrice = decorator.CalculateCost();
            //    fee += decorator.CalculateAdditionalFee();

            //    //Dekorator podatku
            //    decorator = new TaxDecorator(decorator);
            //    finalPrice = decorator.CalculateCost();
            //    fee += decorator.CalculateAdditionalFee();

            //    //Dekorator konwersji PLN to USD
            //    //decorator = new ConversionFromPLNtoUSDDecorator(decorator);
            //    //finalPrice = decorator.CalculateCost();
            //    //fee += decorator.CalculateAdditionalFee();

            //    //Dekorator konwersji PLN to EUR
            //    //decorator = new ConversionFromPLNtoEURDecorator(decorator);
            //    //finalPrice = decorator.CalculateCost();
            //    //fee += decorator.CalculateAdditionalFee();

            //    double formattedFee;
            //    double formattedFinalPrice;

            //    double.TryParse(String.Format("{0:0.##}", fee), out formattedFee);
            //    double.TryParse(String.Format("{0:0.##}", finalPrice), out formattedFinalPrice);

            //    quoteWSignals.AdditionalFee = formattedFee;
            //    quoteWSignals.FinalPrice = formattedFinalPrice;
            //}
            //);

            #endregion

            #region Decorator

            _decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes);
            //_decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes, new List<DecoratorComponent> { new CommissionDecorator(), new TaxDecorator() });

            #endregion

            #region DeepClone

            _deepClone.ChangeClonnedCollection(obtainedSignalsWithQuotes);

            //Deep Clone using JsonSerialization
            List<SignalModelContext> clonedSignalContextByJsonSerializer = _deepClone.DeepCloneUsingJsonSerialization();

            //Deep Clone using BinarySerialization
            List<SignalModelContext> clonedSignalContextByBinarySerializer = _deepClone.DeepCloneUsingBinarySerialization();

            //Deep Clone using Reflection
            List<SignalModelContext> clonedSignalContextByReflection = _deepClone.DeepCloneUsingReflection();

            #endregion

            #region SavingResults

            //Saving JsonFile to PrototypeObjects directory
            _fileHandler.SaveJsonFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

            //Saving CsvFile to GeneratedSignals directory
            _fileHandler.SaveCsvFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

            #endregion
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

            #region ChainOfResponsibility

            _chainOfResponsibility.ExecuteChainOfPartialSignalToDeterminFinalSignal(ref obtainedSignalsWithQuotes);

            #endregion

            #region Decorator

            _decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes);
            //_decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes, new List<DecoratorComponent> { new CommissionDecorator(), new TaxDecorator() });

            #endregion

            #region DeepClone

            _deepClone.ChangeClonnedCollection(obtainedSignalsWithQuotes);

            //Deep Clone using JsonSerialization
            List<SignalModelContext> clonedSignalContextByJsonSerializer = _deepClone.DeepCloneUsingJsonSerialization();

            //Deep Clone using BinarySerialization
            List<SignalModelContext> clonedSignalContextByBinarySerializer = _deepClone.DeepCloneUsingBinarySerialization();

            //Deep Clone using Reflection
            List<SignalModelContext> clonedSignalContextByReflection = _deepClone.DeepCloneUsingReflection();

            #endregion

            #region SavingResults

            //Saving JsonFile to PrototypeObjects directory
            _fileHandler.SaveJsonFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

            //Saving CsvFile to GeneratedSignals directory
            _fileHandler.SaveCsvFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

            #endregion
        }

        public void CountSingleIndicatorForAllCompaniesQuotes(TechnicalIndicator technicalIndicator)
        {
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);
            List<List<Quote>> companiesQuotes = _fileHandler.ReadMultipleCsvFiles(namesOfCompanies);

            int iterationNumber = 0;

            // foreach (var companyName in namesOfCompanies)
            foreach (var companyQuotes in companiesQuotes)
            {
                string nameOfCompany = namesOfCompanies[iterationNumber];

                _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
                List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();
                List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

                #region ChainOfResponsibility

                _chainOfResponsibility.ExecuteChainOfPartialSignalToDeterminFinalSignal(ref obtainedSignalsWithQuotes);

                #endregion

                #region Decorator

                _decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes);
                //_decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes, new List<DecoratorComponent> { new CommissionDecorator(), new TaxDecorator() });

                #endregion

                #region DeepClone

                _deepClone.ChangeClonnedCollection(obtainedSignalsWithQuotes);

                //Deep Clone using JsonSerialization
                List<SignalModelContext> clonedSignalContextByJsonSerializer = _deepClone.DeepCloneUsingJsonSerialization();

                //Deep Clone using BinarySerialization
                List<SignalModelContext> clonedSignalContextByBinarySerializer = _deepClone.DeepCloneUsingBinarySerialization();

                //Deep Clone using Reflection
                List<SignalModelContext> clonedSignalContextByReflection = _deepClone.DeepCloneUsingReflection();

                #endregion

                #region SavingResults

                //Saving JsonFile to PrototypeObjects directory
                _fileHandler.SaveJsonFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

                //Saving CsvFile to GeneratedSignals directory
                _fileHandler.SaveCsvFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

                #endregion

                iterationNumber++;
            }
        }

        public void CountIndicatorsSetForAllCompaniesQuotes()
        {
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);
            List<List<Quote>> companiesQuotes = _fileHandler.ReadMultipleCsvFiles(namesOfCompanies);

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

                #region ChainOfResponsibility

                _chainOfResponsibility.ExecuteChainOfPartialSignalToDeterminFinalSignal(ref obtainedSignalsWithQuotes);

                #endregion

                #region Decorator

                _decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes);
                //_decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes, new List<DecoratorComponent> { new CommissionDecorator(), new TaxDecorator() });

                #endregion

                #region DeepClone

                _deepClone.ChangeClonnedCollection(obtainedSignalsWithQuotes);

                //Deep Clone using JsonSerialization
                List<SignalModelContext> clonedSignalContextByJsonSerializer = _deepClone.DeepCloneUsingJsonSerialization();

                //Deep Clone using BinarySerialization
                List<SignalModelContext> clonedSignalContextByBinarySerializer = _deepClone.DeepCloneUsingBinarySerialization();

                //Deep Clone using Reflection
                List<SignalModelContext> clonedSignalContextByReflection = _deepClone.DeepCloneUsingReflection();

                #endregion

                #region SavingResults

                //Saving JsonFile to PrototypeObjects directory
                _fileHandler.SaveJsonFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

                //Saving CsvFile to GeneratedSignals directory
                _fileHandler.SaveCsvFileWithSignalModelContextObjects(obtainedSignalsWithQuotes, nameOfCompany);

                #endregion

                iterationNumber++;
            }
        }

        #region ParallelVersion

        public void CountSingleIndicatorForAllCompaniesQuotesFullParallelVersion(TechnicalIndicator technicalIndicator)
        {
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);
            List<List<Quote>> companiesQuotes = _fileHandler.ReadMultipleCsvFiles(namesOfCompanies);

            int iterationNumber = 0;

            Parallel.ForEach(companiesQuotes, (companyQuotes) =>
            {
                string nameOfCompany;

                lock (_padlock)
                {
                    nameOfCompany = namesOfCompanies[iterationNumber];
                    iterationNumber++;
                }

                _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, technicalIndicator);
                List<Signal> obtainedSignals = _calculateContext.ReceiveSignalsFromSingleCalculatedIndicator();
                List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

                #region ChainOfResponsibility

                ChainOfResponsibilityFacade chainOfResponsibility = new ChainOfResponsibilityFacade();
                chainOfResponsibility.ExecuteChainOfPartialSignalToDeterminFinalSignal(ref obtainedSignalsWithQuotes);

                #endregion

                #region Decorator

                DecoratorFacade decorator = new DecoratorFacade();
                decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes);
                //decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes, new List<DecoratorComponent> { new CommissionDecorator(), new TaxDecorator() });

                #endregion

                #region DeepClone

                DeepCloneFacade deepClone = new DeepCloneFacade();
                deepClone.ChangeClonnedCollection(obtainedSignalsWithQuotes);

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
            });
        }

        public void CountIndicatorsSetForAllCompaniesQuotesFullParallelVersion()
        {
            List<string> namesOfCompanies = FileHelper.GetFileNames(PathToUnpackedQuotesDirectory);
            List<List<Quote>> companiesQuotes = _fileHandler.ReadMultipleCsvFiles(namesOfCompanies);

            int iterationNumber = 0;

            Parallel.ForEach(companiesQuotes, (companyQuotes) =>
            {
                string nameOfCompany;

                lock (_padlock)
                {
                    nameOfCompany = namesOfCompanies[iterationNumber];
                    iterationNumber++;
                }

                foreach (var indicator in _indicators)
                {
                    _calculateContext.CalculateSingleIndicator(companyQuotes, _parameters, indicator);
                }

                List<List<Signal>> obtainedSignals = _calculateContext.ReceiveSignalsFromCalculatedIndicators(_indicators.Count());
                List<SignalModelContext> obtainedSignalsWithQuotes = AutoMapperHelper.MapQuotesAndSignalsToSignalModelContext(companyQuotes, obtainedSignals);

                #region ChainOfResponsibility

                ChainOfResponsibilityFacade chainOfResponsibility = new ChainOfResponsibilityFacade();
                chainOfResponsibility.ExecuteChainOfPartialSignalToDeterminFinalSignal(ref obtainedSignalsWithQuotes);

                #endregion

                #region Decorator

                DecoratorFacade decorator = new DecoratorFacade();
                decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes);
                //decorator.CalculateAdditionalFeeAndFinalPrice(ref obtainedSignalsWithQuotes, new List<DecoratorComponent> { new CommissionDecorator(), new TaxDecorator() });

                #endregion

                #region DeepClone

                DeepCloneFacade deepClone = new DeepCloneFacade();
                deepClone.ChangeClonnedCollection(obtainedSignalsWithQuotes);

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
            });
        }

        #endregion  
    }
}
