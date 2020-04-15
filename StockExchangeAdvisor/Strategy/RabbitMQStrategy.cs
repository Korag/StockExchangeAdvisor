using AdapterDesignPattern;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using TechnicalIndicators;

namespace Signals
{
    class RabbitMQStrategy : ICalculateTechnicalIndicatorStrategy
    {
        private const string _exchange = "SignalsExchange";
       
        public RabbitMQStrategy()
        {

        }

        public List<Signal> ReceiveData()
        {
            throw new NotImplementedException();
        }
                                                                  // do wykorzystania później
        public void SendData(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            RabbitMQSendQuotesAdapter adapter = new RabbitMQSendQuotesAdapter();

            adapter.SendQuotesToCalculationInConsumer(quotes, parameters, _exchange, indicator.GetType().ToString());
        }

        
    }
}
