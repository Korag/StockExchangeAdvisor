using RabbitMQ;
using TechnicalIndicators;

namespace RabbitMQConsumerROC2
{
    public class RabbitConsumerCalculateROC2
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
            TechnicalIndicator _indicator = new TechnicalIndicatorROC();
            InititalizeParameters(_indicator.GetType().ToString());

            RabbitCalculateIndicator rabbitROC2 = new RabbitCalculateIndicator(_exchange, _queueReceiveFrom, _queueSendTo, _indicator);
            rabbitROC2.ConsumeData();
        }
    }
}
