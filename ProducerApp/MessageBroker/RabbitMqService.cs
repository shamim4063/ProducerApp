using Newtonsoft.Json;
using System.Text;

namespace Utilities.MessageBroker
{
    public interface IRabbitMQService
    {
        void PublishMessage<T>(string exchangeName, string queueName, T message);
    }

    public class RabbitMQService : IRabbitMQService
    {
        public RabbitMQService()
        {
            RabbitMQConnectionProvider.Connect();
        }

        public void PublishMessage<T>(string exchangeName, string queueName, T message)
        {
            try
            {
                var channel = RabbitMQConnectionProvider.GetChannel();

                if (channel != null)
                {
                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchangeName, queueName, true, RabbitMQConnectionProvider.GetProperties(), body);              
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }            
        }
    }
}
