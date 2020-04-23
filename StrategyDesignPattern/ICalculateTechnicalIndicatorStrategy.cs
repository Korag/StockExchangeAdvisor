using Models;
using System.Collections.Generic;
using TechnicalIndicators;

namespace StrategyDesignPattern
{
    public interface ICalculateTechnicalIndicatorStrategy
    {
        void SendData(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator);
        List<Signal> ReceiveData();
    }
}
