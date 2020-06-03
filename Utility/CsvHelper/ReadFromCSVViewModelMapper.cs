using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Models;

namespace Utility
{
    public sealed class ReadFromCSVViewModelMapper : ClassMap<Quote>
    {
        public ReadFromCSVViewModelMapper()
        {
            Map(m => m.Date).Name("<DTYYYYMMDD>");
            Map(m => m.Open).Name("<OPEN>");
            Map(m => m.High).Name("<HIGH>");
            Map(m => m.Low).Name("<LOW>");
            Map(m => m.Close).Name("<CLOSE>");
           // Map(m => m.Volume).Name("<VOL>").ConvertUsing(row => (int)row.GetField<double>("<VOL>"));
            Map(m => m.Volume).ConvertUsing(row => (int)row.GetField<double>("<VOL>"));
        }
    }

}
