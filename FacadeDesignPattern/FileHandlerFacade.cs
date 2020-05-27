using Models;
using StateAndDecoratorDesignPattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Utility;

namespace FacadeDesignPattern
{
    public class FileHandlerFacade
    {
        private Object _padlock { get; set; }

        public FileHandlerFacade()
        {
            _padlock = new object();
        }

        public List<List<Quote>> ReadMultipleCsvFiles(List<string> namesOfCompanies)
        {
            List<List<Quote>> companiesQuotes = new List<List<Quote>>();

            Parallel.ForEach(namesOfCompanies, (companyName) =>
            {
                var companyQuotes = Utility.CsvHelper.ReadSingleCsvFileWithQuotes(companyName);

                lock (_padlock)
                {
                    companiesQuotes.Add(companyQuotes);
                }
            });

            return companiesQuotes;
        }

        public void SaveJsonFileWithSignalModelContextObjects(List<SignalModelContext> signalsWithQuotes, string nameOfCompany)
        {
            string jsonString = JsonSerializer.ConvertCollectionOfObjectsToJsonString<SignalModelContext>(signalsWithQuotes);
            DateTime currentDateTime = DateTime.Now;
            string dateTimeFormat = "ddMMyyyy-HHmm";
            string fileName = nameOfCompany + "_" + currentDateTime.ToString(dateTimeFormat) + "-" +
                                Convert.ToString((int)currentDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) + "_prototypeObject";

            FileHelper.SaveJsonFile(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\StockExchangeAdvisor\\PrototypeObjects\\{fileName}.json")), jsonString);
        }

        public void SaveCsvFileWithSignalModelContextObjects(List<SignalModelContext> signalsWithQuotes, string nameOfCompany)
        {
            DateTime currentDateTime = DateTime.Now;
            string dateTimeFormat = "ddMMyyyy-HHmm";
            string fileName = nameOfCompany + "_" + currentDateTime.ToString(dateTimeFormat) + "-" +
                                Convert.ToString((int)currentDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) + "_generatedSignals";

            Utility.CsvHelper.SaveCompanySignalsToCsvFile(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"..\\..\\..\\..\\StockExchangeAdvisor\\GeneratedSignals\\{fileName}.csv")), signalsWithQuotes);
        }
    }
}
