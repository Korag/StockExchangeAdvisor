
using Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using Utility;

namespace RabbitMQ
{
    public class RabbitMQReceiveObtainedSignals : IRabbitMQReceiveObtainedSignals
    {
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

                        for (int i = 0; i < countedTechnicalIndicatorsNumber; i++)
                        {
                            BasicGetResult data;

                            do
                            {
                                data = channel.BasicGet(_queueReceiveFrom, false);

                            } while (data == null);

                            List<Signal> currentReceivedSignals = JsonSerializer.JsonStringToCollectionOfObjectsTypes<Signal>(EncryptionHelper.ByteArrayToUTF8String(data.Body));
                            _obtainedSignals.Add(currentReceivedSignals);

                            Console.WriteLine("RabbitMQ Producer received signals from Consumers.");
                            channel.BasicAck(data.DeliveryTag, false);
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
    }
}
