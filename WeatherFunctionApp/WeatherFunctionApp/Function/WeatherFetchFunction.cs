using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using WeatherFunctionApp.Model;

namespace WeatherFunctionApp.Function
{
    public class WeatherFetchFunction
    {
        private readonly IWeatherService _weatherService;

        public WeatherFetchFunction(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [FunctionName("WeatherFetchFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timer, ILogger log)
        {
            log.LogInformation("Fetching weather data...");
            await _weatherService.FetchAndStoreWeatherDataAsync();
        }
    }
}
