using RabbitMQ.Client;

namespace Utility
{
    public static class RabbitMQConnectionHelper
    {
        // dodać wczytywanie wartości z pliku json, aby można było je zmieniać

        public static IConnection GetConnection()
        {
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                Protocol = Protocols.DefaultProtocol,
                VirtualHost = "/",
                UserName = "guest",
                Password = "guest"
            };
            return connectionFactory.CreateConnection();
        }
    }
}
