using Models;
using System.Collections.Generic;

namespace TechnicalIndicators
{
    public abstract class RSI : TechnicalIndicator
    {

        private const int SELL = -1;
        private const int STAY = 0;
        private const int BUY = 1;

        public override List<Signal> GetSignals(List<Quote> quote, Parameters parameters)
        {
            List<Signal> Signal_test = new List<Signal>();

            for (int i = parameters.NQuotesBackwards; i < quote.Count; i++)
            {
                Signal signal_1 = new Signal
                {
                    Date = quote[i].Date
                };

                double value, sum, value_1, sum_1;
                //double[] rise_values =  quote[i].Close_Rise;                                //Wartości wzrostu cen
                //double rise_count = rise_values.Length;                                     //Ilość wartości wzrostu cen
                //double[] decrease_values = quote[i].Close_Decrease;                         //Wartości spadku cen
                //double decrease_count = decrease_values.Length;                             //Ilość wartości spadku cen
                double a = 0;
                double b = 0;

                //for (value = 0; value <= rise_count - 1; value++)                           //Pętla z której otrzymujemy wartość a = średnia wartość wzrostu cen zamknięcia
                //{
                //    sum += rise_values[1];
                //    a = sum / rise_count;
                //}

                //for (value_1 = 0; value_1 <= decrease_count - 1; value_1++)                 //Pętla z której otrzymujemy wartość b = średnia wartość spadku cen zamknięcia
                //{
                //    sum_1 += decrease_values[1];
                //    b = sum / decrease_count;
                //}

                double rs = a / b;                                                          //Wyliczanie rs, potrzebne do wyliczenia wartość RSI
                double rsi = 100 - 100 / (1 + rs);                                          //Wyliczanie wartości RSI

                if (rsi <= 30)                                                              //Jeżeli wartość RSI jest mniejsza niż 30, jest to sygnał kupna
                {
                    signal_1.Value = BUY;
                }

                else if (rsi >= 70)                                                         //Jeżeli wartość RSI jest większa niż 70, jest to sygnał sprzedaży
                {
                    signal_1.Value = SELL;
                }
                
                else if (rsi < 70 || rsi > 30)                                              //Jeżeli wartość RSI zawiera się pomiędzy 30 a 70, nie należy podejmować żadnych decyzji
                {
                    signal_1.Value = STAY;
                }

                Signal_test.Add(signal_1);
            }

            return Signal_test;
        }
    }
}
