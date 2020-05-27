using Models;
using RabbitMQ.Client;
using System;
using Utility;

namespace RabbitMQ
{
    public class RabbitMQSendQuotesToCalculation : IRabbitMQSendQuotesToCalculation
    {
        public RabbitMQSendQuotesToCalculation(string exchange, string queueSendTo) : base(exchange, queueSendTo)
        {

        }

        public override void Process(IndicatorCalculationElements elements)
        {
            try
            {
                using (var connection = RabbitMQConnectionHelper.GetConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //umożliwia potwierdzanie, że przesyłkę dostarczono do Exchange
                        channel.ConfirmSelect();

                        //generujemy nową wiadomość
                        GenerateAndPublishMessage(channel, elements);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public override void GenerateAndPublishMessage(IModel channel, IndicatorCalculationElements indicatorElements)
        {
            //serializujemy oraz przekształcamy w tablicę bajtów
            var body = EncryptionHelper.StringToUtf8(JsonSerializer.ConvertObjectToJsonString<IndicatorCalculationElements>(indicatorElements));
            channel.BasicPublish(exchange: _exchange,
                routingKey: _queueSendTo,
                basicProperties: null,
                body: body);
            //oczekujemy na potwierdzenie, że przesyłka dotarła do exchange
            channel.WaitForConfirmsOrDie();
            Console.WriteLine($"Przesłałem wskaźnik do obliczenia");
        }
    }
}
