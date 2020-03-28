using System.Collections.Generic;
using System.Linq;

namespace Signals
{
    class TechnicalIndicatorEMA : TechnicalIndicator
    {
        private const int BUY_SIGNAL = -1;
        private const int SELL_SIGNAL = 1;

        class IndicatorValueWithDate
        {
            public double IndicatorValue;
            public string Date;
        }

        public override List<Signal> GetSignals(List<Quote> quotes, Parameters parameters)
        {
            int quoteQuantity = quotes.Count;
            quotes = quotes.OrderBy(z => z.Date).ToList();

            double previousEMA = 0;
            double currentEMA;
            double multiplier = (2d / (parameters.Period + 1d));

            List<IndicatorValueWithDate> calculatedEMA = new List<IndicatorValueWithDate>();

            for (int i = 0; i < quoteQuantity; i++)
            {
                if (i == 0)
                {
                    double calculatedSMA = quotes.Take(parameters.Period).Average(z => z.Close);
                    currentEMA = calculatedSMA;
                    i = parameters.Period - 1;
                }
                else
                {
                    currentEMA = (quotes[i].Close - previousEMA) * multiplier + previousEMA;
                }
                previousEMA = currentEMA;

                calculatedEMA.Add(new IndicatorValueWithDate
                {
                    IndicatorValue = currentEMA,
                    Date = quotes[i].Date
                });
            }

            List<Signal> obtainedSignals = new List<Signal>();
            int quotesWithoutCalculatedIndicator = parameters.Period - 1;

            for (int i = 0; i < calculatedEMA.Count; i++)
            {
                Signal newSignal = new Signal
                {
                    Date = calculatedEMA[i].Date
                };

                if (quotes[quotesWithoutCalculatedIndicator + i].Close > calculatedEMA[i].IndicatorValue)
                {
                    newSignal.Value = BUY_SIGNAL;
                }
                else if (quotes[quotesWithoutCalculatedIndicator + i].Close <= calculatedEMA[i].IndicatorValue)
                {
                    newSignal.Value = SELL_SIGNAL;
                }

                obtainedSignals.Add(newSignal);
            };

            return obtainedSignals;
        }
    }
}



