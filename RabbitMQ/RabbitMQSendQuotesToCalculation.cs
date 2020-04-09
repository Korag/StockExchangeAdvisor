//using Models;
//using RabbitMQ.Client;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Utility;

//namespace RabbitMQ
//{
//    class RabbitMQSendQuotesToCalculation
//    {
//        private void Process(IndicatorCalculationElements elements)
//        {
//            try
//            {
//                using (var connection = RabbitMQConnectionHelper.GetConnection())
//                {
//                    using (var channel = connection.CreateModel())
//                    {
//                        //umożliwia potwierdzanie, że przesyłkę dostarczono do Exchange
//                        channel.ConfirmSelect();
//                        while (true)
//                        {
//                            //generujemy nowe konto
//                            GenerateAndPublishMessage(channel, elements);
//                        }
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw;
//            }
//        }

//        private static void GenerateAndPublishMessage(IModel channel, IndicatorCalculationElements elements)
//        {
//            //serializujemy oraz przekształcamy w tablicę bajtów
//            var body = Utils.StringToUtf8(Utils.AccountToJsonString(newAccount));
//            channel.BasicPublish(exchange: _exchange,
//                routingKey: _routingKey,
//                basicProperties: null,
//                body: body);
//            //oczekujemy na potwierdzenie, że przesyłka dotarła do exchange
//            channel.WaitForConfirmsOrDie();
//            Console.WriteLine($"Dodałem dla: {newAccount.EmailAddress} i passwordHash: {newAccount.PasswordHash}");
//        }
//    }
//}
