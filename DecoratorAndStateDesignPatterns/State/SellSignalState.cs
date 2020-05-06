using DecoratorAndStateDesignPatterns;
using System;

namespace StateDesignPattern
{
    //Concrete state
    public class SellSignalState : ASignalState
    {
        public string Factor
        {
            get { return "proponowana akcja sprzedaży"; }
        }

        public SellSignalState(ASignalState state) : this(state.SignalValue, state.Context)
        {

        }

        public SellSignalState(int signalValue, SignalModelContext context)
        {
            this.Context = context;
            this.SignalValue = signalValue;
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
