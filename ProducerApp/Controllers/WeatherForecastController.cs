using Microsoft.AspNetCore.Mvc;
using Service.MessageBroker.Producers;

namespace ProducerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IGeneralLeadInfoProducer _generalLeadInfoProducer;
        static int _count = 0;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IGeneralLeadInfoProducer generalLeadInfoProducer)
        {
            _logger = logger;
            _generalLeadInfoProducer = generalLeadInfoProducer;
        }


        [HttpGet("/send-message-to-queue-one")]
        public IActionResult SendMessage() 
        {
            _count++;
            Console.WriteLine($"Sending Message No. {_count}");
            _generalLeadInfoProducer.NotifyOnNewMessage($"Message No {_count}");

            return Ok("Success");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
