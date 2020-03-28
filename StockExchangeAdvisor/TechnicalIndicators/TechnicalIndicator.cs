using System.Collections.Generic;

namespace Signals
{
    abstract class TechnicalIndicator
    {
        public abstract List<Signal> GetSignals(List<Quote> q, Parameters p);
    }
}
