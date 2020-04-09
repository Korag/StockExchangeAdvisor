using RabbitMQ.Client;

namespace Utility
{
    public static class RabbitMQConnectionHelper
    {
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
