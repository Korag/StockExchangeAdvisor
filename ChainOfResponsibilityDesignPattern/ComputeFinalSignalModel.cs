using System;
using System.Collections.Generic;

namespace ChainOfResponsibilityDesignPattern
{
    public class ComputeFinalSignalModel
    {
        private const int UNKNOWN_SIGNAL = 0;

        public List<double> PartialSignals { get; set; }
        public int PartialSignalsCount { get; set; }
        public int BuySignalsCount { get; private set; }
        public int SellSignalsCount { get; private set; }

        public int CurrentIndex { get; private set; }
        public double FinalSignal { get; private set; }
        public int CertaintyOfOutcomeCount { get; set; }

        public ComputeFinalSignalModel(List<double> partialSignals)
        {
            PartialSignals = partialSignals;
           
            PartialSignalsCount = PartialSignals.Count;
            BuySignalsCount = 0;
            SellSignalsCount = 0;
            FinalSignal = UNKNOWN_SIGNAL;

            CertaintyOfOutcomeCount = (int)(Math.Ceiling((decimal)PartialSignals.Count/2.0m));
        }

        public void IncrementIndex()
        {
            CurrentIndex++;
        }

        public void IncrementSellSignalCount()
        {
            SellSignalsCount++;
        }

        public void IncrementBuySignalCount()
        {
            BuySignalsCount++;
        }

        public void SetFinalSignalValue(double signalValue)
        {
            FinalSignal  = signalValue;
        }
    }
}
