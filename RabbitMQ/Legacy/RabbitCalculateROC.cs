using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TechnicalIndicators;
using Utility;

namespace RabbitMQUniversalConsumer
{
    public class RabbitCalculateROC : AbstractClass
    {
        public RabbitCalculateROC(string queueReceiveFromName, string queueSendToName, int intervalTime) : base(queueReceiveFromName, queueSendToName, intervalTime)
        {

        }

        public override void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel)
        {
            var quotes = JsonSerializer.JsonStringToCollectionOfQuotes(EncryptionHelper.ByteArrayToUTF8String(ea.Body));
            Parameters p = new Parameters();

            // kalkulacja wybranego wskaźnika
            TechnicalIndicator ema = new TechnicalIndicatorEMA();
            ema.GetSignals(quotes, p);

            channel.BasicAck(ea.DeliveryTag, false);
        }
    }
}
