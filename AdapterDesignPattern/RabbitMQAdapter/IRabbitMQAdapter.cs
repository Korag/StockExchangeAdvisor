using Models;
using System.Collections.Generic;

namespace AdapterDesignPattern
{
    public interface IRabbitMQAdapter
    {
        void InitializeProducer(string exchange, string queueSendTo);
        void SendQuotesToCalculationInConsumer(List<Quote> quotes, Parameters parameters,
                                                     string exchange, string queueSendTo);
    }
}
