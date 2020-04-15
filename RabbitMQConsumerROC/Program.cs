using TechnicalIndicators;
using RabbitMQ;

namespace RabbitMQConsumerROC
{
    class Program
    {
        private const string _queueReceiveFrom = "roc_tobecalculated";
        private const string _queueSendTo = "roc_signals";

        static void Main()
        {
            TechnicalIndicator _indicator = new TechnicalIndicatorROC();
            RabbitCalculateIndicator rabbitROC = new RabbitCalculateIndicator(_queueReceiveFrom, _queueSendTo, _indicator);
            rabbitROC.ConsumeData();
        }
    }
}
