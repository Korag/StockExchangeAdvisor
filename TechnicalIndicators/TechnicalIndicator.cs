using Models;
using System.Collections.Generic;

namespace TechnicalIndicators
{
    public abstract class TechnicalIndicator
    {
        public abstract List<Signal> GetSignals(List<Quote> quote, Parameters parameters);
    }
}
