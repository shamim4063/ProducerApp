using Microsoft.AspNetCore.Mvc.Filters;
using Utilities.MessageBroker;

namespace Service.MessageBroker.Producers
{
    public interface IGeneralLeadInfoProducer {
        void NotifyOnNewMessage(string message);
    }
    public class GeneralLeadInfoProducer : IGeneralLeadInfoProducer
    {
        private readonly IRabbitMQService _rabbitMQService;

        public GeneralLeadInfoProducer(IRabbitMQService rabbitMqService)
        {
            _rabbitMQService = rabbitMqService;
        }

        public void NotifyOnNewMessage(string message)
        {
            try
            {
                _rabbitMQService.PublishMessage("RnDExchange", "Queue1", new { Username = "Shamim", Message = message });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace, ex);
            }
        }
     
    }
}

