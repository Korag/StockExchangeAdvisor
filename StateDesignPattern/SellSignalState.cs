using System;

namespace StateDesignPattern
{
    public class SellSignalState : SignalState
    {
        public SellSignalState(SignalState state) : this(state.SignalValue, state.Context)
        {

        }

        public SellSignalState(int signalValue, SignalStateContext context)
        {
            this.Context = context;
            this.SignalValue = signalValue;
        }

        public override void SaveSignalToFile(string fileURL)
        {
            //write to fileURL with proper state

            throw new NotImplementedException();
        }

        public override void SetSignalValue(int signalValue)
        {
            switch (signalValue)
            {
                case 0:
                    Context.currentState.SignalValue = signalValue;
                    Context.currentState = new UnknownSignalState(this);
                    break;

                case -1:
                    Context.currentState.SignalValue = signalValue;
                    Context.currentState = new BuySignalState(this);
                    break;

                default:
                    break;
            }
        }
    }
}
