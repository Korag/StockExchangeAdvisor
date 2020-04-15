using Models;
using System.Collections.Generic;

namespace AdapterDesignPattern
{
    public interface IRabbitMQSendQuotesAdapter
    {
        void SendQuotesToCalculationInConsumer(List<Quote> quotes, Parameters parameters,
                                                     string exchange, string queueSendTo);
    }
}
