using Models;
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

        public abstract List<List<Signal>> ReceiveObtainedSignals(int countedTechnicalIndicatorsNumber);
    }
}
