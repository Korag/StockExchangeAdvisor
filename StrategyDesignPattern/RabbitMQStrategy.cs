using AdapterDesignPattern;
using Models;
using RabbitMQ;
using System.Collections.Generic;
using TechnicalIndicators;

namespace StrategyDesignPattern
{
    public class RabbitMQStrategy : ICalculateTechnicalIndicatorStrategy
    {
        private string _exchange = "SignalsExchange";
        private string _queueReceiveFrom = "ObtainedSignals";

        public RabbitMQStrategy()
        {
        }

        public RabbitMQStrategy(string exchange, string queueReceiveFrom)
        {
            _exchange = exchange;
            _queueReceiveFrom = queueReceiveFrom;
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
