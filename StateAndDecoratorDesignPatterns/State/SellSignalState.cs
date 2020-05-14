using StateAndDecoratorDesignPattern;
using System;

namespace StateDesignPattern
{
    //Concrete state
    [Serializable]
    public class SellSignalState : ASignalState
    {
        public string Factor
        {
            get { return "proponowana akcja sprzedaży"; }
        }

        public SellSignalState()
        {

        }

        public SellSignalState(ASignalState state) : this(state.SignalValue, state.Context)
        {

        }

        public SellSignalState(double signalValue, SignalModelContext context)
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

                case -1:
                    Context.CurrentState = new BuySignalState(this);
                    break;

                default:
                    break;
            }
        }
    }
}
