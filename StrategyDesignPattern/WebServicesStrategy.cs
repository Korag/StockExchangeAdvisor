using Models;
using System;
using System.Collections.Generic;
using TechnicalIndicators;

namespace StrategyDesignPattern
{
    public class WebServicesStrategy : ICalculateTechnicalIndicatorStrategy
    {
        public List<List<Signal>> ReceiveData(int countedTechnicalIndicatorsNumber)
        {
            throw new NotImplementedException();
        }

        public void SendData(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            throw new NotImplementedException();
        }
    }
}
