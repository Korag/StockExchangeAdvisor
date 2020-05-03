using System.Collections.Generic;

namespace StateDesignPattern
{
    public class SignalModelContext
    {
        //TODO: url
        public string fileURL = "";

        public string Date;
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public int Volume;

        public List<int> PartialSignals;
        public ASignalState currentState = null;

        //Przeniesione do określonych stanów
        //public string Factor;
        //public int FinalSignal; -> SignalValue

        //TODO:
        //decorator prowizja
        //decorator podatek
        //decorator przewalutowanieUSD
        //decorator przewalutowanieEUR

        public SignalModelContext()
        {
            currentState = new UnknownSignalState(0, this);
        }

        public void SetSignalValue(int signalValue)
        {
            currentState.SetSignalValue(signalValue);
        }
    }
}
