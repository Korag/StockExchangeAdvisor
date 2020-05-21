using Models;
using System;
using System.Collections.Generic;

namespace TechnicalIndicators
{
    public class CandleFormation : TechnicalIndicator
    {       
        private const int SELL = -1;
        private const int BUY = 1;

        public CandleFormation()
        {

        }

        public override List<Signal> GetSignals(List<Quote> quote, Parameters parameters)
        {
            List<Signal> SignalList = new List<Signal>();

            /*Quote first = new Quote();
            first.Open = 13.70;
            first.Close = 14.00;
            first.Low = 14.00;
            first.High = 16.00;*/

            for (int i = parameters.NQuotesBackwards; i < quote.Count; i++)
            {
                Signal test = new Signal
                {
                    Date = quote[i].Date
                };

                double shadow_low = quote[i].Close - quote[i].Low;
                double shadow_high = quote[i].High - quote[i].Open;

                double bodySize = Math.Abs(quote[i].Open - quote[i].Close);

                if (shadow_low > bodySize * 2 && shadow_high < bodySize * 0.1)
                {
                    test.Value = SELL;
                }

                else if (shadow_low < bodySize * 0.1 && shadow_high > bodySize * 2)
                {
                    test.Value = BUY;
                }

                SignalList.Add(test);
            }

            return SignalList;
        }
    }
}
