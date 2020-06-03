using ChainOfResponsibilityDesignPattern;
using StateAndDecoratorDesignPattern;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacadeDesignPattern
{
    public class ChainOfResponsibilityFacade
    {
        public void ExecuteChainOfPartialSignalToDeterminFinalSignal(ref List<SignalModelContext> signalsWithQuotes)
        {
            Parallel.ForEach(signalsWithQuotes, (singleQuotePartialSignals) =>
            {
                ConcreteChainHandlerElement chain = new ConcreteChainHandlerElement();

                ComputeFinalSignalModel finalSignalModel = new ComputeFinalSignalModel(singleQuotePartialSignals.PartialSignals);
                singleQuotePartialSignals.SetSignalValue(chain.DetermineFinalSignal(finalSignalModel));
            });
        }
    }
}
