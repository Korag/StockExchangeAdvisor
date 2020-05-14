using System;
using System.Collections.Generic;
using System.Text;

namespace ChainOfResponsibilityDesignPattern
{
    public abstract class AChainHandlerElement
    {
        protected AChainHandlerElement descendantHandlerElement;

        public delegate int OnDetermineOfFinalSignal(AChainHandlerElement handler/*, Leave l*/, int number);
        public event OnDetermineOfFinalSignal onDetermination = null;

        public AChainHandlerElement DescendantHandlerElement
        {
            get
            {
                return descendantHandlerElement;
            }
            set
            {
                descendantHandlerElement = value;
            }
        }

        public int DetermineOfFinalSignal(AChainHandlerElement handler/*, Leave leave*/, int number)
        {
            if (onDetermination != null)
            {
                onDetermination(this/*, leave*/, number);
            }

            return number;
        }

        public int DetermineFinalSignal(/*Leave l*/)
        {
             return DetermineOfFinalSignal(this/*, l*/, 0);
        }

        public abstract int ReturnDeterminedFinalSignal(int finalSignal);
    }
}
