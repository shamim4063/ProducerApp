using RabbitMQ.Client;
using System;


namespace Utilities.MessageBroker
{
    public class RabbitMQConnectionProvider
    {
        private static IConnection _connection;
        private static IModel _channel;
        private static IBasicProperties _properties;
        private static readonly object _lock = new object();

        public static void Connect(string exchangeName = "GeneralLeadExchange")
        {
            if (_connection == null)
            {
                lock (_lock)
                {
                    if (_connection == null)
                    {
                        try
                        {
                            var factory = new ConnectionFactory()
                            {
                                HostName = "95.45.224.19",
                                Port = 5672,
                                VirtualHost = "qsmartdev",
                                UserName = "essadmin",
                                Password = "123@qwe"
                            };

                            _connection = factory.CreateConnection();
                            _channel = _connection.CreateModel();
                            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
                            _properties = _channel.CreateBasicProperties();
                            _properties.Persistent = true;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to connect RabbitMQ {ex.Message}");
                        }
                    }
                }
            }
        }

        public static IModel GetChannel()
        {
            return _channel;
        }

        public static IBasicProperties GetProperties()
        {
            return _properties;
        }

        public static void Dispose()
        {
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
            }

            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
            }

            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
