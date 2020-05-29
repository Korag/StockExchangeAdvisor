using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using TechnicalIndicators;
using Utility;

namespace RabbitMQ
{
    public class RabbitCalculateIndicator : IRabbitCalculateIndicator
    {
        private TechnicalIndicator _indicator;

        public RabbitCalculateIndicator(string exchange, string queueReceiveFrom, string queueSendTo, TechnicalIndicator indicator) : base(exchange, queueReceiveFrom, queueSendTo)
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
                throw e;
            }
        }

        public override void GenerateAndPublishMessage(IModel channel, List<Signal> obtainedSignals)
        {
            //serializujemy oraz przekształcamy w tablicę bajtów
            var body = EncryptionHelper.StringToUtf8(JsonSerializer.ConvertCollectionOfObjectsToJsonString<Signal>(obtainedSignals));
            channel.BasicPublish(exchange: _exchange,
                routingKey: _queueSendTo,
                basicProperties: null,
                body: body);
            //oczekujemy na potwierdzenie, że przesyłka dotarła do exchange
            Console.WriteLine($"RabbitMQ Consumer sent obtained signals to Exchange.");
            channel.WaitForConfirmsOrDie();
        }

        public override void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel)
        {
            var indicatorElements = JsonSerializer.JsonStringToObjectType<IndicatorCalculationElements>(EncryptionHelper.ByteArrayToUTF8String(ea.Body));

            List<Signal> obtainedSignals = _indicator.GetSignals(indicatorElements.Quotes, indicatorElements.Parameters);
            Console.WriteLine("RabbitMQ Consumer calculated Technical Indicator."); 

            channel.BasicAck(ea.DeliveryTag, false);
            GenerateAndPublishMessage(channel, obtainedSignals);
        }
    }
}