using CsvHelper.Configuration;
using StateAndDecoratorDesignPattern;

namespace Utility
{
    public sealed class WriteSignalModelContextMapper : ClassMap<SignalModelContext>
    {
        public WriteSignalModelContextMapper()
        {
            Map(m => m.Date).Name("<DTYYYYMMDD>").Index(0); ;
            Map(m => m.Open).Name("<OPEN>").Index(1);
            Map(m => m.High).Name("<HIGH>").Index(2);
            Map(m => m.Low).Name("<LOW>").Index(3);
            Map(m => m.Close).Name("<CLOSE>").Index(4);
            Map(m => m.Volume).Name("<VOL>").Index(5);

            Map(m => m.CurrentState.SignalValue).Name("<SIGNAL>").Index(6);
            Map(m => m.CurrentState.Factor).Name("<FACTOR>").Index(7);

            Map(m => m.AdditionalFee).Name("<FEE>").Index(8);
            Map(m => m.FinalPrice).Name("<FINALPRICE>").Index(9);
        }
    }
}
