using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using WeatherFunctionApp.Model;

namespace WeatherFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton((s) => {
                return new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            });
            builder.Services.AddSingleton((s) => {
                return new TableServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            });
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IWeatherRepository, WeatherRepository>();
            builder.Services.AddSingleton<IWeatherService, WeatherService>();
        }
    }
}
