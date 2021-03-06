﻿using Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TechnicalIndicators
{
    public abstract class TechnicalIndicatorSMA: TechnicalIndicator
    {
        public TechnicalIndicatorSMA()
        {

        }

		public override List<Signal> GetSignals(List<Quote> quote, Parameters parameters)
        {
            List<Signal> SignalList = new List<Signal>();
            DateTime[] DateList = new DateTime[quote.Count];
            double[] averageArray = new double[quote.Count];
            for (int w = 0; w < quote.Count; w++)
            {
                DateList[w] = DateTime.ParseExact(quote[w].Date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            for (int i = 0; i < quote.Count; i++)
            {
                DateTime getDate = DateTime.ParseExact(quote[i].Date, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime getStartPeriod = getDate.AddDays(-parameters.Period);
                DateTime getEndPeriod = getDate.AddDays(parameters.Period);
                double sumForGivenIteration = 0;
                int counter = 0;
                for (int k = 0; k < quote.Count; k++)
                {

                    if (DateList[k] < getEndPeriod && DateList[k] > getStartPeriod)
                    {
                        sumForGivenIteration += quote[k].High + quote[k].Low;
                        counter++;
                    }
                }
                averageArray[i] = sumForGivenIteration / counter;
            }
            for (int i = 0; i < quote.Count; i++)
            {
                Signal signalToAdd = new Signal();
                signalToAdd.Date = quote[i].Date;
                if (averageArray[i] > quote[i].Close)
                {
                    signalToAdd.Value = -1;
                }
                if (averageArray[i] < quote[i].Close)
                {
                    signalToAdd.Value = 1;
                }
                SignalList.Add(signalToAdd);
            }

            return SignalList;
        }
    }
}
