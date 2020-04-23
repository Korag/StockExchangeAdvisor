using Models;
using System;
using System.Collections.Generic;
using TechnicalIndicators;

namespace StrategyDesignPattern
{
    public class ActorModelStrategy : ICalculateTechnicalIndicatorStrategy
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
