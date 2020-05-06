
using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace RabbitMQ
{
    public class RabbitMQReceiveObtainedSignals : IRabbitMQReceiveObtainedSignals
    {
        //private List<Signal> _obtainedSignals = new List<Signal>();

        private List<List<Signal>> _obtainedSignals = new List<List<Signal>>();

        public RabbitMQReceiveObtainedSignals(string exchange, string queueReceiveFrom) : base(exchange, queueReceiveFrom)
        {
        }

        public override List<List<Signal>> ReceiveObtainedSignals(int countedTechnicalIndicatorsNumber)
        {
            GetObtainedSignalsFromQueue(countedTechnicalIndicatorsNumber);
            return _obtainedSignals;
        }

        private void GetObtainedSignalsFromQueue(int countedTechnicalIndicatorsNumber)
        {
            try
            {
                using (var connection = RabbitMQConnectionHelper.GetConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //ustalamy,że będzie pobierał jeden item z kolejki i przetwarzał
                        channel.BasicQos(0, 1, false);

                        //var consumer = new EventingBasicConsumer(channel);
                        //consumer.Received += (model, ea) =>
                        //{
                        //    HandleReceivedEvent(ea, channel, connection);
                        //};
                        ////podłączenie konsumenta do kolejki
                        //channel.BasicConsume(queue: _queueReceiveFrom,
                        //   consumer: consumer);
                        for (int i = 0; i < countedTechnicalIndicatorsNumber; i++)
                        {
                            var data = channel.BasicGet(_queueReceiveFrom, false);

                            if (data != null)
                            {
                                List<Signal> currentReceivedSignals = JsonSerializer.JsonStringToCollectionOfSignals(EncryptionHelper.ByteArrayToUTF8String(data.Body));
                                _obtainedSignals.Add(currentReceivedSignals);

                                Console.WriteLine("Odebrałem otrzymane sygnały z kolejki");
                                channel.BasicAck(data.DeliveryTag, false);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //public override void HandleReceivedEvent(BasicDeliverEventArgs ea, IModel channel, IConnection connection)
        //{
        //    _obtainedSignals = JsonSerializer.JsonStringToCollectionOfSignals(EncryptionHelper.ByteArrayToUTF8String(ea.Body));
        //    Console.WriteLine("Odebrałem otrzymane sygnały z kolejki");

        //    if (_obtainedSignals.Count != 0)
        //    {
        //        channel.BasicCancel(ea.ConsumerTag);
        //    }

        //    channel.BasicAck(ea.DeliveryTag, false);
        //}
    }
}
