using Models;
using RabbitMQ.Client;

namespace RabbitMQ
{
    public abstract class IRabbitMQSendQuotesToCalculation
    {
        public string _exchange;
        public string _queueSendTo;

        public IRabbitMQSendQuotesToCalculation(string exchange, string queueSendTo)
        {
            _exchange = exchange;
            _queueSendTo = queueSendTo;
        }

        public abstract void Process(IndicatorCalculationElements elements);
        public abstract void GenerateAndPublishMessage(IModel channel, IndicatorCalculationElements elements);
    }
}
