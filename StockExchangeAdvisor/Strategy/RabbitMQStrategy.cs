using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Signals
{
    class RabbitMQStrategy : ICalculateTechnicalIndicatorStrategy
    {
        private const string _exchange = "SignalsExchange";
        public const string _routingKey = "Signal";
        public const int _interval = 1000;

        public List<Signal> ReceiveData()
        {
            throw new NotImplementedException();
        }

        public void SendData(Quote quotes, Parameters parameters)
        {
            throw new NotImplementedException();
        }

        //private void Process()
        //{
        //    try
        //    {
        //        Console.WriteLine($"Będę produkował zadania dodania nowego konta co {_interval} ms");
        //        using (var connection = ConnectionFactoryNamespace.ConnectionFactory.GetConnection())
        //        using (var channel = connection.CreateModel())
        //        {
        //            //umożliwia potwierdzanie, że przesyłkę dostarczono do Exchange
        //            channel.ConfirmSelect();
        //            while (true)
        //            {
        //                //generujemy nowe konto
        //                GenerateAndPublishMessage(channel);
        //                Thread.Sleep(_interval);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        //private static void GenerateAndPublishMessage(IModel channel)
        //{
        //    //serializujemy oraz przekształcamy w tablicę bajtów
        //    var body = Utils.StringToUtf8(Utils.AccountToJsonString(newAccount));
        //    channel.BasicPublish(exchange: _exchange,
        //        routingKey: _routingKey,
        //        basicProperties: null,
        //        body: body);
        //    //oczekujemy na potwierdzenie, że przesyłka dotarła do exchange
        //    channel.WaitForConfirmsOrDie();
        //    Console.WriteLine($"Dodałem dla: {newAccount.EmailAddress} i passwordHash: {newAccount.PasswordHash}");
        //}
    }
}
