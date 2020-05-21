using StateAndDecoratorDesignPattern;
using System;

namespace StateDesignPattern
{
    //Concrete state
    [Serializable]
    public class BuySignalState : ASignalState
    {
        public override string Factor
        {
            get { return "proponowana akcja kupna"; }
        }

        public BuySignalState()
        {

        }

        public BuySignalState(ASignalState state) : this(state.SignalValue, state.Context)
        {

        }

        public BuySignalState(double signalValue, SignalModelContext context)
        {
            this.Context = context;
            this.SignalValue = signalValue;
        }

        public override void SetSignalValue(double signalValue)
        {
            Context.CurrentState.SignalValue = signalValue;

            switch (signalValue)
            {
                case 0:
                    Context.CurrentState = new UnknownSignalState(this);
                    break;

                case 1:
                    Context.CurrentState = new SellSignalState(this);
                    break;

                default:
                    break;
            }
        }
    }
}
