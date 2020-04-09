using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Signals
{
    sealed class CalculateTechnicalIndicatorContext
    {
        private ICalculateTechnicalIndicatorStrategy _strategy = null;
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

        public void CalculateSingleIndicator(List<Quote> quotes, Parameters parameters, Type indicatorType)
        {
            /*_strategy.SendData();*/
            _strategy.ReceiveData();
        }
    }
}
