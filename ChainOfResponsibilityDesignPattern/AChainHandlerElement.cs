namespace ChainOfResponsibilityDesignPattern
{
    public abstract class AChainHandlerElement
    {
        protected AChainHandlerElement descendantHandlerElement;

        public delegate double OnDetermineOfFinalSignal(AChainHandlerElement handler, ComputeFinalSignalModel model);
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

        public double DetermineOfFinalSignal(AChainHandlerElement handler, ComputeFinalSignalModel model)
        {
            if (onDetermination != null)
            {
                onDetermination(this, model);
            }

            return model.FinalSignal;
        }

        public double DetermineFinalSignal(ComputeFinalSignalModel model)
        {
             return DetermineOfFinalSignal(this, model);
        }
    }
}
