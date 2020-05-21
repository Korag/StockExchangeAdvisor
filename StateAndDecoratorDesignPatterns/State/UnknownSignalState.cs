using StateAndDecoratorDesignPattern;
using System;

namespace StateDesignPattern
{
    //Concrete state
    [Serializable]
    public class UnknownSignalState : ASignalState
    {
        public override string Factor
        {
            get { return "brak proponowanej akcji"; }
        }

        public UnknownSignalState()
        {

        }

        public UnknownSignalState(ASignalState state) : this(state.SignalValue, state.Context)
        {

        }

        public UnknownSignalState(double signalValue, SignalModelContext context)
        {
            this.Context = context;
            this.SignalValue = signalValue;
        }

        public override void SetSignalValue(double signalValue)
        {
            Context.CurrentState.SignalValue = signalValue;

            switch (signalValue)
            {
                case 1:
                    Context.CurrentState = new SellSignalState(this);
                    break;

                case -1:
                    Context.CurrentState = new BuySignalState(this);
                    break;

                default:
                    break;
            }
        }
    }
}
