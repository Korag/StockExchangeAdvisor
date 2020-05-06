using System.Collections.Generic;

namespace DecoratorDesignPattern
{
    //State context class
    //Decorator concrete component
    public class SignalModelContext : AFinalPriceComponent
    {
        //TODO: url
        public string fileURL = "";

        public string Date;
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public int Volume;

        public List<double> PartialSignals;
        public ASignalState currentState = null;

        //Przeniesione do określonych stanów
        //public string Factor;
        //public int FinalSignal; -> SignalValue

        public SignalModelContext()
        {
            currentState = new UnknownSignalState(0, this);
            PartialSignals = new List<double>();
        }

        //public void SetSignalValue(int signalValue)
        //{
        //    currentState.SetSignalValue(signalValue);
        //}

        //TODO:
        //decorator prowizja
        //decorator podatek
        //decorator przewalutowanieUSD
        //decorator przewalutowanieEUR

        //Decorator
        private double? _price;

        public override double GetFinalPrice()
        {
            if (_price != null)
            {
                return (double)_price;
            }
            else
            {
                return Close;
            }
        }
    }
}
