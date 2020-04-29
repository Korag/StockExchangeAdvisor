namespace StateDesignPattern
{
    public class SignalStateContext
    {
        public SignalState currentState = null;

        //TODO: url
        public string fileURL = "";

        public string Date;
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public int Volume;

        public SignalStateContext()
        {
            currentState = new UnknownSignalState(0, this);
        }

        public void SetSignalValue(int signalValue)
        {
            currentState.SetSignalValue(signalValue);
        }
    }
}
