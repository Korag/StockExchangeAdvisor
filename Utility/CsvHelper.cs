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
        public static List<Quote> ReadSingleCsvFileWithQuotes(string path)
        {
            try
            {
                IEnumerable<Quote> readQuotes = new List<Quote>();

                using (var reader = new StreamReader(path))
                {
                    // parsing of date

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
                throw;
            }
        }
    }

}
