using TechnicalIndicators;
using RabbitMQ;

namespace RabbitMQConsumerEMA
{
    public class RabbitConsumerCalculateEMA
    {
        private const string _queueReceiveFrom = "ema_tobecalculated";
        private const string _queueSendTo = "ema_signals";
        private const int _interval = 1000;
 
        static void Main()
        {
            TechnicalIndicator _indicator = new TechnicalIndicatorEMA();
            RabbitCalculateIndicator rabbitEMA = new RabbitCalculateIndicator(_queueReceiveFrom, _queueSendTo, _interval, _indicator);
            rabbitEMA.ConsumeData();
        }
    }
}
