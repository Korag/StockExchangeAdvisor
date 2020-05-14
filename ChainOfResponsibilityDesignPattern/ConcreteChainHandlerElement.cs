using System;
using System.Collections.Generic;
using System.Text;

namespace ChainOfResponsibilityDesignPattern
{
    public class ConcreteChainHandlerElement : AChainHandlerElement
    {
        public ConcreteChainHandlerElement()
        {
            this.onDetermination += new OnDetermineOfFinalSignal(ConcreteChainHandlerElement_onDetermination);
        }

        public int ConcreteChainHandlerElement_onDetermination(AChainHandlerElement e/*, Leave l*/, int number)
        {
            // check if we can process this request
            if (true)
            {
                // process it on our level only
                number = ReturnDeterminedFinalSignal(1);
            }
            else
            {
                    DescendantHandlerElement = new ConcreteChainHandlerElement();
                    DescendantHandlerElement.DetermineOfFinalSignal(this/*, l*/, number);
            }

            return number;
        }

        public override int ReturnDeterminedFinalSignal(int finalSignal)
        {
            return finalSignal;
        }
    }
}
