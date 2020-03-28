using System.Collections.Generic;

namespace Signals
{
    class ROC : TechnicalIndicator
    {
        class ROCList
        {
            internal string Date;
            internal double Value;
        }

        public override List<Signal> GetSignals(List<Quote> quote, Parameters parameters)
        {
            var ROCList = new List<ROCList>();

            for (int i = parameters.NQuotesBackwards; i < quote.Count; i++)
            {
                var currentQuote = quote[i].Close;
                var previousQuote = quote[i - parameters.NQuotesBackwards].Close;

                double ROC = (currentQuote - previousQuote) /
                                previousQuote;

                ROCList.Add(new ROCList{Date = quote[i].Date, Value = ROC});
            }

            var signalsList = new List<Signal>();

            foreach (var ROC in ROCList)
            {
                if (ROC.Value >= parameters.BuyTrigger)
                {
                    signalsList.Add(new Signal { Date = ROC.Date, Value = 1 });
                }
                else if (ROC.Value <= parameters.SellTrigger)
                {
                    signalsList.Add(new Signal { Date = ROC.Date, Value = -1 });
                }
            }

            return signalsList;
        }
    }
}