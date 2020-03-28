using CsvHelper.Configuration;

namespace Signals
{
    public class Quote
    {
        public string Date;
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public int Volume;
    }

    public sealed class ReadFromCSVViewModelMapper : ClassMap<Quote>
    {
        public ReadFromCSVViewModelMapper()
        {
            Map(m => m.Date).Name("<DTYYYYMMDD>");
            Map(m => m.Open).Name("<OPEN>");
            Map(m => m.High).Name("<HIGH>");
            Map(m => m.Low).Name("<LOW>");
            Map(m => m.Close).Name("<CLOSE>");
            Map(m => m.Volume).Name("<VOL>");
        }
    }
}
