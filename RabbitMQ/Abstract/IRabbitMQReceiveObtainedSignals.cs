using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;

namespace RabbitMQ
{
    public abstract class IRabbitMQReceiveObtainedSignals
    {
        public string _exchange;
        public string _queueReceiveFrom;

        public IRabbitMQReceiveObtainedSignals(string exchange, string queueReceiveFrom)
        {
            _exchange = exchange;
            _queueReceiveFrom = queueReceiveFrom;
        }

        public abstract List<Signal> ReceiveObtainedSignals();
        public abstract void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel, IConnection connection);
    }
}
