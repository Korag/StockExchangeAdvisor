using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using TechnicalIndicators;
using Utility;

namespace RabbitMQ
{
    public class RabbitCalculateIndicator : IRabbitCalculateIndicator
    {
        private TechnicalIndicator _indicator;

        public RabbitCalculateIndicator(string queueReceiveFrom, string queueSendTo, TechnicalIndicator indicator) : base(queueReceiveFrom, queueSendTo)
        {
            _indicator = indicator;
        }

        public override void ConsumeData()
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

        public override void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel)
        {
            var indicatorElements = JsonSerializer.JsonStringToCollectionOfQuotesWithParameters(EncryptionHelper.ByteArrayToUTF8String(ea.Body));

            _indicator.GetSignals(indicatorElements.Quotes, indicatorElements.Parameters);
            Console.WriteLine("Obliczyłem wskaźnik"); 

            channel.BasicAck(ea.DeliveryTag, false);
        }
    }
}