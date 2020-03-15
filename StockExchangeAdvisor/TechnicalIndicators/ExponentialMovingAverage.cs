using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchangeAdvisor.TechnicalIndicators
{
    class ExponentialMovingAverage : TechnicalIndicator
    {
        private const int BUY_SIGNAL = 1;
        private const int SELL_SIGNAL = -1;
        private const int NEUTRAL_SIGNAL = 0;

        public override List<Signal> GetSignals(List<Quote> q, Parameters p)
        {
            if (p.CalculatedIndicatorFirstDaysInterval == p.CalculatedIndicatorSecondDaysInterval)
            {
                throw new Exception("Both DaysIntervals parameters have the same value. The signals cannot be generated.");
            }

            q = q.OrderBy(z => z.Date).ToList();

            IndicatorCollectionOfValuesWithDaysInterval calculatedEAMForFirstDaysInterval = new IndicatorCollectionOfValuesWithDaysInterval
            {
                IndicatorValues = CalculateExponentialMovingAverage(q, p.CalculatedIndicatorFirstDaysInterval),
                DaysInterval = p.CalculatedIndicatorFirstDaysInterval
            };
            IndicatorCollectionOfValuesWithDaysInterval calculatedEAMForSecondDaysInterval = new IndicatorCollectionOfValuesWithDaysInterval
            {
                IndicatorValues = CalculateExponentialMovingAverage(q, p.CalculatedIndicatorSecondDaysInterval),
                DaysInterval = p.CalculatedIndicatorSecondDaysInterval
            };

            List<Signal> obtainedSignals = DetermineSignalsFromIntersectionOfAverages(calculatedEAMForFirstDaysInterval, calculatedEAMForSecondDaysInterval);

            return obtainedSignals;
        }

        private List<Signal> DetermineSignalsFromIntersectionOfAverages(
                             IndicatorCollectionOfValuesWithDaysInterval firstEAMIndicatorDayInterval,
                             IndicatorCollectionOfValuesWithDaysInterval secondEAMIndicatorDayInterval)
        {
            IndicatorCollectionOfValuesWithDaysInterval shortEAMDayInterval;
            IndicatorCollectionOfValuesWithDaysInterval longEAMDayInterval;
            List<Signal> calculatedSignals = new List<Signal>();

            if (firstEAMIndicatorDayInterval.DaysInterval < secondEAMIndicatorDayInterval.DaysInterval)
            {
                shortEAMDayInterval = firstEAMIndicatorDayInterval;
                longEAMDayInterval = secondEAMIndicatorDayInterval;
            }
            else
            {
                shortEAMDayInterval = secondEAMIndicatorDayInterval;
                longEAMDayInterval = firstEAMIndicatorDayInterval;
            }

            int daysDifferentialWithoutIntersection = shortEAMDayInterval.IndicatorValues.Count() - longEAMDayInterval.IndicatorValues.Count();

            for (int i = 0; i <= daysDifferentialWithoutIntersection; i++)
            {
                calculatedSignals.Add(new Signal
                {
                    Date = shortEAMDayInterval.IndicatorValues[i].Date,
                    Value = NEUTRAL_SIGNAL
                });
            };

            int longEAMDayIntervalLength = longEAMDayInterval.IndicatorValues.Count;

            for (int i = 1; i < longEAMDayIntervalLength; i++)
            {
                calculatedSignals.Add(new Signal
                {
                    Date = longEAMDayInterval.IndicatorValues[i].Date
                });

                if (shortEAMDayInterval.IndicatorValues[i - 1].IndicatorValue > longEAMDayInterval.IndicatorValues[i - 1].IndicatorValue
                    && shortEAMDayInterval.IndicatorValues[i].IndicatorValue < longEAMDayInterval.IndicatorValues[i].IndicatorValue)
                {
                    calculatedSignals[daysDifferentialWithoutIntersection + i].Value = SELL_SIGNAL;
                }
                else if (shortEAMDayInterval.IndicatorValues[i - 1].IndicatorValue < longEAMDayInterval.IndicatorValues[i - 1].IndicatorValue
                    && shortEAMDayInterval.IndicatorValues[i].IndicatorValue > longEAMDayInterval.IndicatorValues[i].IndicatorValue)
                {
                    calculatedSignals[daysDifferentialWithoutIntersection + i].Value = BUY_SIGNAL;
                }
                else
                {
                    calculatedSignals[daysDifferentialWithoutIntersection + i].Value = NEUTRAL_SIGNAL;
                }
            }

            return calculatedSignals;
        }

        private double ExponentialMovingAverageFormula(double currentQuoteClosePrice, double previousEMA, int daysInterval)
        {
            double multiplier = (2d / (daysInterval + 1d));
            double currentEMA = (currentQuoteClosePrice - previousEMA) * multiplier + previousEMA;

            return currentEMA;
        }

        private List<IndicatorValueWithDate> CalculateExponentialMovingAverage(List<Quote> analyzedQuotes, int daysInterval)
        {
            if (analyzedQuotes.Count >= daysInterval)
            {
                double previousEMA = 0;
                double currentEMA;
                List<IndicatorValueWithDate> calculatedEMAs = new List<IndicatorValueWithDate>();

                for (int i = 0; i < analyzedQuotes.Count; i++)
                {
                    if (i == 0)
                    {
                        currentEMA = SimpleMovingAverageFormula(analyzedQuotes.Take(daysInterval).ToList());
                        i = daysInterval - 1;
                    }
                    else
                    {
                        currentEMA = ExponentialMovingAverageFormula(analyzedQuotes[i].Close, previousEMA, daysInterval);
                    }
                    previousEMA = currentEMA;

                    calculatedEMAs.Add(new IndicatorValueWithDate
                    {
                        IndicatorValue = currentEMA,
                        Date = analyzedQuotes[i].Date
                    });
                }

                return calculatedEMAs;
            }
            else
            {
                throw new FormatException("List of Quotes has less elements than entered DaysInterval attribute.");
            }
        }

        private double SimpleMovingAverageFormula(List<Quote> analyzedQuotes)
        {
            double calculatedSMA = analyzedQuotes.Average(z => z.Close);
            return calculatedSMA;
        }
    }

    class IndicatorValueWithDate
    {
        public double IndicatorValue;
        public string Date;
    }

    class IndicatorCollectionOfValuesWithDaysInterval
    {
        public List<IndicatorValueWithDate> IndicatorValues;
        public int DaysInterval;
    }
}



