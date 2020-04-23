using AdapterDesignPattern;
using Models;
using RabbitMQ;
using System.Collections.Generic;
using TechnicalIndicators;

namespace StrategyDesignPattern
{
    public class RabbitMQStrategy : ICalculateTechnicalIndicatorStrategy
    {
        private const string _exchange = "SignalsExchange";
        private const string _queueReceiveFrom = "ObtainedSignals";

        public RabbitMQStrategy()
        {

        }

        public List<Signal> ReceiveData()
        {
            IRabbitMQReceiveObtainedSignals signalConsumer = new RabbitMQReceiveObtainedSignals(_exchange, _queueReceiveFrom);
            return signalConsumer.ReceiveObtainedSignals();
        }
                                                                  
        public void SendData(List<Quote> quotes, Parameters parameters, TechnicalIndicator indicator)
        {
            RabbitMQSendQuotesAdapter adapter = new RabbitMQSendQuotesAdapter();

            adapter.SendQuotesToCalculationInConsumer(quotes, parameters, _exchange, indicator.GetType().ToString());
        }
    }
}
