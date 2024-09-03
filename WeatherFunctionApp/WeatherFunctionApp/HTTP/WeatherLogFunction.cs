using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeatherFunctionApp.Model;

namespace WeatherFunctionApp.HTTP
{
    public class WeatherLogFunction
    {
        private readonly IWeatherService _weatherService;

        public WeatherLogFunction(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [FunctionName("GetLogs")]
        public async Task<IActionResult> GetLogs(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "logs")] HttpRequest req, ILogger log)
        {
            DateTime from = DateTime.Parse(req.Query["from"]);
            DateTime to = DateTime.Parse(req.Query["to"]);

            var logs = await _weatherService.GetLogsAsync(from, to);
            return new OkObjectResult(logs);
        }
    }
}
