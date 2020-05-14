namespace ChainOfResponsibilityDesignPattern
{
    public class ConcreteChainHandlerElement : AChainHandlerElement
    {
        private const int BUY_SIGNAL = -1;
        private const int SELL_SIGNAL = 1;
        private const int UNKNOWN_SIGNAL = 0;

        public ConcreteChainHandlerElement()
        {
            this.onDetermination += new OnDetermineOfFinalSignal(ConcreteChainHandlerElement_onDetermination);
        }

        public double ConcreteChainHandlerElement_onDetermination(AChainHandlerElement handler, ComputeFinalSignalModel model)
        {
            if (model.CurrentIndex < model.PartialSignalsCount)
            {
                if (model.BuySignalsCount >= model.CertaintyOfOutcomeCount)
                {
                    model.SetFinalSignalValue(BUY_SIGNAL);
                }
                else if (model.SellSignalsCount >= model.CertaintyOfOutcomeCount)
                {
                    model.SetFinalSignalValue(SELL_SIGNAL);
                }
                else
                {
                    switch (model.PartialSignals[model.CurrentIndex])
                    {
                        case BUY_SIGNAL:
                            model.IncrementBuySignalCount();
                            break;

                        case SELL_SIGNAL:
                            model.IncrementSellSignalCount();
                            break;
                    }

                    model.IncrementIndex();
                    DescendantHandlerElement = new ConcreteChainHandlerElement();
                    DescendantHandlerElement.DetermineOfFinalSignal(this, model);
                }
            }
            else
            {
                if (model.BuySignalsCount == model.SellSignalsCount)
                {
                    model.SetFinalSignalValue(UNKNOWN_SIGNAL);
                }
                else
                {
                    model.SetFinalSignalValue(model.BuySignalsCount > model.SellSignalsCount ? BUY_SIGNAL : SELL_SIGNAL);
                }
            }

            return model.FinalSignal;
        }
    }
}
