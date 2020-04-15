using TechnicalIndicators;
using RabbitMQ;

namespace RabbitMQConsumerROC
{
    class Program
    {
        private const string _exchange = "SignalsExchange";
        private const string _queueSendTo = "Signals";
        private static string _queueReceiveFrom;

        static void InititalizeParameters(string queueReceiverFrom)
        {
            _queueReceiveFrom = queueReceiverFrom;
        }

        static void Main()
        {
            TechnicalIndicator _indicator = new TechnicalIndicatorROC();
            RabbitCalculateIndicator rabbitROC = new RabbitCalculateIndicator(_exchange, _queueReceiveFrom, _queueSendTo, _indicator);
            rabbitROC.ConsumeData();
        }
    }
}
