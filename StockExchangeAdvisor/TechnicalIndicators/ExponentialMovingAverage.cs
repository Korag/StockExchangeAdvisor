using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockExchangeAdvisor.TechnicalIndicators
{
    class ExponentialMovingAverage : TechnicalIndicator
    {
        public override List<Signal> GetSignals(List<Quote> q, Parameters p)
        {
            if (p.CalculatedIndicatorFirstDaysInterval == p.CalculatedIndicatorSecondDaysInterval)
            {
                throw new Exception("Both DaysIntervals have the same value. The signals cannot be generated");
            }

            q = q.OrderBy(z => z.Date).ToList();

            IndicatorCollectionOfValuesWithDaysInterval calculatedEAMForFirstDaysInterval = new IndicatorCollectionOfValuesWithDaysInterval
            {
                IndicatorValues = CalculateExponentialMovingAverage(q, p.CalculatedIndicatorFirstDaysInterval),
                DaysInterval = p.CalculatedIndicatorSecondDaysInterval
            };
            IndicatorCollectionOfValuesWithDaysInterval calculatedEAMForSecondDaysInterval = new IndicatorCollectionOfValuesWithDaysInterval
            {
                IndicatorValues = CalculateExponentialMovingAverage(q, p.CalculatedIndicatorSecondDaysInterval),
                DaysInterval = p.CalculatedIndicatorSecondDaysInterval
            };

            //List<Signal> obtainedSignals = DetermineSignalsFromIntersectionOfAverages();
            //return obtainedSignals;

            throw new NotImplementedException();
        }

        private double ExponentialMovingAverageFormula(double currentQuoteClosePrice, double previousEMA, int daysInterval)
        {
            double multiplier = (2 / (daysInterval + 1));
            double currentEMA = (currentQuoteClosePrice - previousEMA) * multiplier + previousEMA;

            return currentEMA;
        }

        private List<IndicatorValueWithDaysIntervalAndDate> CalculateExponentialMovingAverage(List<Quote> analyzedQuotes, int daysInterval)
        {
            if (analyzedQuotes.Count >= daysInterval)
            {
                double previousEMA = 0;
                double currentEMA = 0;
                List<IndicatorValueWithDaysIntervalAndDate> calculatedEMAs = new List<IndicatorValueWithDaysIntervalAndDate>();

                for (int i = 0; i < daysInterval; i++)
                {
                    if (i == 0)
                    {
                        previousEMA = SimpleMovingAverageFormula(analyzedQuotes.Take(daysInterval).ToList());
                        i = daysInterval-1;
                        continue;
                    }
                    currentEMA = ExponentialMovingAverageFormula(analyzedQuotes[i].Close, previousEMA, daysInterval);
                    previousEMA = currentEMA;

                    calculatedEMAs.Add(new IndicatorValueWithDaysIntervalAndDate
                    {
                        Value = currentEMA,
                        Date = analyzedQuotes[i].Date
                    });
                }

                return calculatedEMAs;
            }
            else
                throw new FormatException("List of Quotes has less elements than entered daysInterval argument");
        }

        private double SimpleMovingAverageFormula(List<Quote> analyzedQuotes)
        {
            double calculatedSMA = analyzedQuotes.Average(z=> z.Close);
            return calculatedSMA;
        }
    }

    class IndicatorValueWithDaysIntervalAndDate
    {
        public string Date;
        public double Value;
    }

    class IndicatorCollectionOfValuesWithDaysInterval
    {
        public List<IndicatorValueWithDaysIntervalAndDate> IndicatorValues;
        public int DaysInterval;
    }
}



