using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchangeAdvisor.TechnicalIndicators
{
    abstract class TechnicalIndicator
    {
        public abstract List<Signal> GetSignals(List<Quote> q, Parameters p);
    }
}
