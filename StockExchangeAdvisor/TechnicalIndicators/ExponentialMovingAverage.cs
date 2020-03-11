using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockExchangeAdvisor.TechnicalIndicators
{
    class ExponentialMovingAverage : TechnicalIndicator
    {
        private const int BUY_SIGNAL = 1;
        private const int SELL_SIGNAL = -1;
        private const int NOT_ENOUGH_DATA = 0;

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
                DaysInterval = p.CalculatedIndicatorSecondDaysInterval
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

            int daysDifferentialWithoutIntersection = shortEAMDayInterval.IndicatorValues.Count() - longEAMDayInterval.IndicatorValues.Count() + 1;

            for (int i = 0; i < daysDifferentialWithoutIntersection; i++)
            {
                calculatedSignals[i] = new Signal
                {
                    Date = shortEAMDayInterval.IndicatorValues[i].Date,
                    Value = NOT_ENOUGH_DATA
                };
            };

            int longEAMDayIntervalLength = longEAMDayInterval.IndicatorValues.Count;

            for (int i = 1; i < longEAMDayIntervalLength; i++)
            {
                calculatedSignals[daysDifferentialWithoutIntersection + i] = new Signal
                {
                    Date = longEAMDayInterval.IndicatorValues[i].Date
                };

                if (shortEAMDayInterval.IndicatorValues[i-1].Value > longEAMDayInterval.IndicatorValues[i-1].Value
                    && shortEAMDayInterval.IndicatorValues[i].Value < longEAMDayInterval.IndicatorValues[i].Value)
                {
                    calculatedSignals[daysDifferentialWithoutIntersection + i].Value = SELL_SIGNAL;
                }
                else if (shortEAMDayInterval.IndicatorValues[i - 1].Value < longEAMDayInterval.IndicatorValues[i - 1].Value
                    && shortEAMDayInterval.IndicatorValues[i].Value > longEAMDayInterval.IndicatorValues[i].Value)
                {
                    calculatedSignals[daysDifferentialWithoutIntersection + i].Value = BUY_SIGNAL;
                }
                else
                {
                    calculatedSignals[daysDifferentialWithoutIntersection + i].Value = NOT_ENOUGH_DATA;
                }
            }

            return calculatedSignals;
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
                throw new FormatException("List of Quotes has less elements than entered DaysInterval argument.");
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



