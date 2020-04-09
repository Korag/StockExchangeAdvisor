using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using Utility;

namespace RabbitMQUniversalConsumer
{
    public abstract class AbstractClass
    {
        private string _queueReceiveFrom = "ema_tobecalculated";
        private string _queueSendTo = "ema_signals";
        private int _interval = 1000;

        public AbstractClass(string queueReceiveFromName, string queueSendToName, int intervalTime)
        {
            _queueReceiveFrom = queueReceiveFromName;
            _queueSendTo = queueSendToName;
            _interval = intervalTime;
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

        public abstract void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel);
    }
}