using CsvHelper;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Utility
{
    public static class CsvHelper
    {
        private static string _quotesURL { get => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\QuotesDownloader\\DownloadedQuotes\\"));}
        private static string _generatedSignalsURL { get => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\StockExchangeAdvisor\\GeneratedSignals\\")); }

        public static List<Quote> ReadSingleCsvFileWithQuotes(string nameOfCompany)
        {
            try
            {
                IEnumerable<Quote> readQuotes = new List<Quote>();

                using (var reader = new StreamReader(_quotesURL + nameOfCompany + ".mst"))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.HasHeaderRecord = true;
                        csv.Configuration.RegisterClassMap<ReadFromCSVViewModelMapper>();
                        csv.Configuration.Delimiter = ",";

                        readQuotes = csv.GetRecords<Quote>();

                        return readQuotes.ToList();
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"The file for company {0} does not exist.", nameOfCompany);
                throw;
            }
        }

        public static void SaveCompanySignalsToCsvFile(List<QuoteWithSignal> quotesWithSignal, string nameOfCompany)
        {
            try
            {
                using (var writer = new StreamWriter(_generatedSignalsURL + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString() + "_" + nameOfCompany + ".csv"))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteComment(nameOfCompany);
                        csv.WriteComment("___________");
                        csv.WriteRecords(quotesWithSignal);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error during creating signals file");
                throw;
            }
        }
    }

}
