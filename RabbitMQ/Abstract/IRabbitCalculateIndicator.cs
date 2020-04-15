using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;

namespace RabbitMQ
{
    public abstract class IRabbitCalculateIndicator
    {
        public string _exchange;
        public string _queueReceiveFrom;
        public string _queueSendTo;

        public IRabbitCalculateIndicator(string exchange, string queueReceiveFrom, string queueSendTo)
        {
            _exchange = exchange;
            _queueReceiveFrom = queueReceiveFrom;
            _queueSendTo = queueSendTo;
        }

        public abstract void ConsumeData();
        public abstract void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel);
        public abstract void GenerateAndPublishMessage(IModel channel, List<Signal> obtainedSignals);
    }
}
