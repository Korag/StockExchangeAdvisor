using CsvHelper;
using Models;
using StateAndDecoratorDesignPattern;
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

        public static void SaveCompanySignalsToCsvFile(string URL, List<SignalModelContext> model)
        {
            try
            {
                using (var writer = new StreamWriter(URL))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.HasHeaderRecord = true;
                        csv.Configuration.RegisterClassMap<WriteSignalModelContextMapper>();
                        csv.Configuration.Delimiter = ",";

                        csv.WriteRecords(model);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error during creation of signals file");
                throw e;
            }
        }
    }

}
