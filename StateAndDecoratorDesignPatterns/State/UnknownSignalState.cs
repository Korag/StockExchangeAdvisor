using StateAndDecoratorDesignPattern;
using System;

namespace StateDesignPattern
{
    //Concrete state
    [Serializable]
    public class UnknownSignalState : ASignalState
    {
        public string Factor
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

        public override void SetSignalValue(int signalValue)
        {
            switch (signalValue)
            {
                case 1:
                    Context.CurrentState.SignalValue = signalValue;
                    Context.CurrentState = new SellSignalState(this);
                    break;

                case -1:
                    Context.CurrentState.SignalValue = -signalValue;
                    Context.CurrentState = new BuySignalState(this);
                    break;

                default:
                    break;
            }
        }
    }
}
