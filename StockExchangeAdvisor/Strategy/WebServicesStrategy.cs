using Models;
using System;
using System.Collections.Generic;
using TechnicalIndicators;

namespace Signals
{
    class WebServicesStrategy : ICalculateTechnicalIndicatorStrategy
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
