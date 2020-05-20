using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TechnicalIndicators;
using Utility;
using WebService;
using WebServicesModels;

namespace AdapterDesignPattern
{
    public class WebServiceAdapter : IWebServiceAdapter
    {
        private List<int> _generatedId { get; set; }
        private IWebServiceCalculateIndicator _webService { get; set; }

        public WebServiceAdapter()
        {
            _generatedId = new List<int>();
            _webService = new WebServiceCalculateIndicator();
        }

        public void SendQuotesToCalculationOnWebService(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            IndicatorCalculationElementsWIndicatorType data = new IndicatorCalculationElementsWIndicatorType
            {
                Quotes = quotes,
                Parameters = parameters,
                TechnicalIndicator = indicator
            };

            _generatedId.Add(_webService.CalculateTechnicalIndicator(data));
        }

        public List<List<Signal>> ReceiveObtainedSignalsFromWebService(int countedTechnicalIndicatorsNumber)
        {
            if (_generatedId.Count() != countedTechnicalIndicatorsNumber)
            {
                throw new ArgumentException();
            }

            List<List<Signal>> obtainedSignals = new List<List<Signal>>();

            foreach (var id in _generatedId)
            {
                string jsonString = _webService.GetObtainedSignals(id);
                //deserialization problem
                List<Signal> partialSignal = JsonSerializer.JsonStringToCollectionOfObjectsTypes<Signal>(jsonString);
                obtainedSignals.Add(partialSignal);
            }

            _generatedId = new List<int>();
            return obtainedSignals;
        }
    }
}
