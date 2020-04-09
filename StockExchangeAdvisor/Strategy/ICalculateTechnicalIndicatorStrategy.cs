using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Signals
{
    interface ICalculateTechnicalIndicatorStrategy
    {
        public void SendData(Quote quotes, Parameters parameters);
        public List<Signal> ReceiveData();
    }
}
