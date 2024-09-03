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
    public class WeatherPayloadFunction
    {
        private readonly IWeatherService _weatherService;

        public WeatherPayloadFunction(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [FunctionName("GetPayload")]
        public async Task<IActionResult> GetPayload(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "payload/{id}")] HttpRequest req, string id, ILogger log)
        {
            var payload = await _weatherService.GetPayloadAsync(id);
            if (payload == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(payload);
        }
    }

}
