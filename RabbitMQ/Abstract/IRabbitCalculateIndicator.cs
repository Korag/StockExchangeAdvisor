using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ
{
    public abstract class IRabbitCalculateIndicator
    {
        public string _queueReceiveFrom;
        public string _queueSendTo;

        public IRabbitCalculateIndicator(string queueReceiveFrom, string queueSendTo)
        {
            _queueReceiveFrom = queueReceiveFrom;
            _queueSendTo = queueSendTo;
        }

        public abstract void ConsumeData();
        public abstract void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel);
    }
}
