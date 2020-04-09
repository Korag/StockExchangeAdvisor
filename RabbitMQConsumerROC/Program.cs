using TechnicalIndicators;
using RabbitMQ;

namespace RabbitMQConsumerROC
{
    class Program
    {
        private const string _queueReceiveFrom = "roc_tobecalculated";
        private const string _queueSendTo = "roc_signals";
        private const int _interval = 1000;

        static void Main()
        {
            TechnicalIndicator _indicator = new TechnicalIndicatorROC();
            RabbitCalculateIndicator rabbitROC = new RabbitCalculateIndicator(_queueReceiveFrom, _queueSendTo, _interval, _indicator);
            rabbitROC.ConsumeData();
        }
    }
}
