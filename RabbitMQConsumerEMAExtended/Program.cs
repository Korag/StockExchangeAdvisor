using RabbitMQ;
using TechnicalIndicators;

namespace RabbitMQConsumerEMAExtended
{
    public class RabbitConsumerCalculateEMAExtended
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
            TechnicalIndicator _indicator = new ExponentialMovingAverage();
            InititalizeParameters(_indicator.GetType().ToString());

            RabbitCalculateIndicator rabbitEMAExtended = new RabbitCalculateIndicator(_exchange, _queueReceiveFrom, _queueSendTo, _indicator);
            rabbitEMAExtended.ConsumeData();
        }
    }
}
