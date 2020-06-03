using Models;
using System.Collections.Generic;
using TechnicalIndicators;

namespace StrategyDesignPattern
{
    public sealed class CalculateTechnicalIndicatorContext
    {
        private static ICalculateTechnicalIndicatorStrategy _strategy = null;
        private static CalculateTechnicalIndicatorContext instance = null;

        private CalculateTechnicalIndicatorContext(ICalculateTechnicalIndicatorStrategy strategy)
        {
            _strategy = strategy;
        }

        public static CalculateTechnicalIndicatorContext GetInstance(ICalculateTechnicalIndicatorStrategy strategy)
        {
            if (instance == null)
            {
                instance = new CalculateTechnicalIndicatorContext(strategy);
            }

            return instance;
        }

        public void ChangeStrategy(ICalculateTechnicalIndicatorStrategy strategy)
        {
            _strategy = strategy;
        }

        public void CalculateSingleIndicator(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            _strategy.SendData(quotes, parameters, indicator);
        }

        public List<Signal> ReceiveSignalsFromSingleCalculatedIndicator()
        {
            List<List<Signal>> generatedSignals = _strategy.ReceiveData(1);

            if (generatedSignals.Count > 0)
            {
                return generatedSignals[0];
            }
            else
            {
                return new List<Signal>();
            }
        }

        public List<List<Signal>> ReceiveSignalsFromCalculatedIndicators(int countedTechnicalIndicatorsNumber)
        {
            return _strategy.ReceiveData(countedTechnicalIndicatorsNumber);
        }
    }
}
