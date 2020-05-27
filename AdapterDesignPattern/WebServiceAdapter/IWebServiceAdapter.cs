using Models;
using System.Collections.Generic;
using TechnicalIndicators;

namespace AdapterDesignPattern
{
    public interface IWebServiceAdapter
    {
        void SendQuotesToCalculationOnWebService(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator);
        List<List<Signal>> ReceiveObtainedSignalsFromWebService(int countedTechnicalIndicatorsNumber);
    }
}
