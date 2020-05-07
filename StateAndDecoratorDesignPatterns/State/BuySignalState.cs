using StateAndDecoratorDesignPattern;
using System;

namespace StateDesignPattern
{
    //Concrete state
    [Serializable]
    public class BuySignalState : ASignalState
    {
        public string Factor
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

        public override void SetSignalValue(int signalValue)
        {
            switch (signalValue)
            {
                case 0:
                    Context.CurrentState.SignalValue = signalValue;
                    Context.CurrentState = new UnknownSignalState(this);
                    break;

                case 1:
                    Context.CurrentState.SignalValue = signalValue;
                    Context.CurrentState = new SellSignalState(this);
                    break;

                default:
                    break;
            }
        }
    }
}
