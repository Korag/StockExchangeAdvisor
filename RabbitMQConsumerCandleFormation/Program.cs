using RabbitMQ;
using TechnicalIndicators;

namespace RabbitMQConsumerCandleFormation
{
    public class RabbitConsumerCalculateCandleFormation
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
            TechnicalIndicator _indicator = new CandleFormation();
            InititalizeParameters(_indicator.GetType().ToString());

            RabbitCalculateIndicator rabbitCandleFormation = new RabbitCalculateIndicator(_exchange, _queueReceiveFrom, _queueSendTo, _indicator);
            rabbitCandleFormation.ConsumeData();
        }
    }
}
