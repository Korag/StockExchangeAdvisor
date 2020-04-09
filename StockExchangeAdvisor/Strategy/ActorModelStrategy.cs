using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Signals
{
    class ActorModelStrategy : ICalculateTechnicalIndicatorStrategy
    {
        public List<Signal> ReceiveData()
        {
            throw new NotImplementedException();
        }

        public void SendData(Quote quotes, Parameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
