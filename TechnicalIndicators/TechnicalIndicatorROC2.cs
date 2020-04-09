using Models;
using System.Collections.Generic;

namespace TechnicalIndicators
{
    // https://tradersarea.pl/wskaznik-zmiany-rate-of-change-roc/
    // https://atskaner.pl/baza/oscylatory-techniczne-momentum-roc-i-macd-1
    // https://www.edukacjagieldowa.pl/gieldowe-abc/analiza-techniczna/narzedzia-analizy-technicznej/roc/

    public class TechnicalIndicatorROC : TechnicalIndicator
    {
        public override List<Signal> GetSignals(List<Quote> quote, Parameters parameters)
        {
            int p = parameters.Period, n = quote.Count;
            double[] values = new double[quote.Count];
            var ret = new List<Signal>();

            // Calc ROC from close value
            for (int i = p; i < n; i++)
            {
                values[i] = quote[i].Close - quote[i - p].Close;
            }

            // Interpret ROC Value and generate signals
            for (int i = 1; i < n; i++)
            {
                if (values[i] > 0 && values[i - 1] < 0 || // ROC raise and cross zero - buy
                    values[i] < 0 && values[i - 1] > 0) // ROC fall and cross zero - sell
                {
                    ret.Add(new Signal { 
                       // company = quote[i].company, 
                        Date = quote[i].Date, 
                        Value = values[i] - values[i - 1] });
                }
            }

            return ret;
        }
    }
}
