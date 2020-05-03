using System;

namespace StateDesignPattern
{
    public class BuySignalState : ASignalState
    {
        public override string Factor
        {
            get { return "proponowana akcja kupna"; }
        }

        public BuySignalState(ASignalState state) : this(state.SignalValue, state.Context)
        {

        }

        public BuySignalState(int signalValue, SignalModelContext context)
        {
            this.Context = context;
            this.SignalValue = signalValue;
        }

        public override void SaveSignalToFile(string fileURL)
        {
            //write to fileURL with proper state only one line

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

                case 1:
                    Context.currentState.SignalValue = signalValue;
                    Context.currentState = new SellSignalState(this);
                    break;

                default:
                    break;
            }
        }
    }
}
