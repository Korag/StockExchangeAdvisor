using Models;
using System;
using System.Collections.Generic;
using System.Text;
using TechnicalIndicators;

namespace Signals
{
    interface ICalculateTechnicalIndicatorStrategy
    {
        public void SendData(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator);
        public List<Signal> ReceiveData();
    }
}
