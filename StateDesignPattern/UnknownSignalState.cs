using System;

namespace StateDesignPattern
{
    public class UnknownSignalState : SignalState
    {
        public UnknownSignalState(SignalState state) : this(state.SignalValue, state.Context)
        {

        }

        public UnknownSignalState(int signalValue, SignalStateContext context)
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
                case 1:
                    Context.currentState.SignalValue = signalValue;
                    Context.currentState = new SellSignalState(this);
                    break;

                case -1:
                    Context.currentState.SignalValue = -signalValue;
                    Context.currentState = new BuySignalState(this);
                    break;

                default:
                    break;
            }
        }
    }
}
