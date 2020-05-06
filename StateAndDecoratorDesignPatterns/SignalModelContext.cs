using StateDesignPattern;
using System.Collections.Generic;

namespace StateAndDecoratorDesignPattern
{
    //State context class
    public class SignalModelContext
    {
        public string Date;
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public int Volume;

        public List<double> PartialSignals;
        public ASignalState CurrentState = null;

        //Wykorzystanie decoratora do obliczenia
        public double FinalPrice;
        public double AdditionalFee;

        //Przeniesione do określonych stanów
        //public string Factor;
        //public int FinalSignal; -> SignalValue

        public SignalModelContext()
        {
            CurrentState = new UnknownSignalState(0, this);
            PartialSignals = new List<double>();
        }

        public void SetSignalValue(int signalValue)
        {
            CurrentState.SetSignalValue(signalValue);
        }
    }
}
