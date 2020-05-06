using System;

namespace StateDesignPattern
{
    //Concrete state
    public class UnknownSignalState : ASignalState
    {
        public override string Factor
        {
            get { return "brak proponowanej akcji"; }
        }

        public UnknownSignalState(ASignalState state) : this(state.SignalValue, state.Context)
        {

        }

        public UnknownSignalState(int signalValue, SignalModelContext context)
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
