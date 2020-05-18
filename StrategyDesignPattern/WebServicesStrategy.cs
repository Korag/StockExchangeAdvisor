using AdapterDesignPattern;
using Models;
using System.Collections.Generic;
using TechnicalIndicators;

namespace StrategyDesignPattern
{
    public class WebServicesStrategy : ICalculateTechnicalIndicatorStrategy
    {
        private WebServiceAdapter _adapter { get; set; }

        public WebServicesStrategy()
        {
            _adapter = new WebServiceAdapter();
        }

        public List<List<Signal>> ReceiveData(int countedTechnicalIndicatorsNumber)
        {
            return _adapter.ReceiveObtainedSignalsFromWebService(countedTechnicalIndicatorsNumber);
        }

        public void SendData(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            _adapter.SendQuotesToCalculationOnWebService(quotes, parameters, indicator);
        }
    }
}
