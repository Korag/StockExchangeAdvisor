using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using TechnicalIndicators;
using Utility;

namespace RabbitMQ
{
    public class RabbitCalculateIndicator
    {
        private string _queueReceiveFrom;
        private string _queueSendTo;
        private int _interval = 1000;

        private TechnicalIndicator _indicator;

        public RabbitCalculateIndicator(string queueReceiveFromName, string queueSendToName, int intervalTime, TechnicalIndicator indicator)
        {
            _queueReceiveFrom = queueReceiveFromName;
            _queueSendTo = queueSendToName;
            _interval = intervalTime;

            _indicator = indicator;
        }

        public virtual void ConsumeData()
        {
            try
            {
                using (var connection = RabbitMQConnectionHelper.GetConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //ustalamy,że będzie pobierał jeden item z kolejki i przetwarzał
                        channel.BasicQos(0, 1, false);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            HandleReceivedEvent(ea, channel);
                        };
                        //podłączenie konsumenta do kolejki
                        channel.BasicConsume(queue: _queueReceiveFrom,
                           consumer: consumer);
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel)
        {
            var quotes = JsonSerializer.JsonStringToCollectionOfQuotes(EncryptionHelper.ByteArrayToUTF8String(ea.Body));
            Parameters p = new Parameters();

            _indicator.GetSignals(quotes, p);

            channel.BasicAck(ea.DeliveryTag, false);
        }
    }
}