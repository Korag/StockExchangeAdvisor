using Models;
using System;
using System.Collections.Generic;
using System.Text;
using TechnicalIndicators;

namespace Signals
{
    class ActorModelStrategy : ICalculateTechnicalIndicatorStrategy
    {
        public List<Signal> ReceiveData()
        {
            throw new NotImplementedException();
        }

        public void SendData(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            throw new NotImplementedException();
        }
    }
}
