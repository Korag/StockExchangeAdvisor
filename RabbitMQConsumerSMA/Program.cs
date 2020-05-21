using RabbitMQ;
using TechnicalIndicators;

namespace RabbitMQConsumerSMA
{
    public class RabbitConsumerCalculateSMA
    {
        private const string _exchange = "SignalsExchange";
        private const string _queueSendTo = "ObtainedSignals";
        private static string _queueReceiveFrom;

        static void InititalizeParameters(string queueReceiverFrom)
        {
            _queueReceiveFrom = queueReceiverFrom;
        }

        static void Main()
        {
            TechnicalIndicator _indicator = new TechnicalIndicatorSMA();
            InititalizeParameters(_indicator.GetType().ToString());

            RabbitCalculateIndicator rabbitSMA = new RabbitCalculateIndicator(_exchange, _queueReceiveFrom, _queueSendTo, _indicator);
            rabbitSMA.ConsumeData();
        }
    }
}
