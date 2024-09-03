using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static WeatherFunctionApp.Model.WeatherData;

namespace WeatherFunctionApp.Model
{
    public interface IWeatherService
    {
        Task FetchAndStoreWeatherDataAsync();
        Task<IEnumerable<WeatherLog>> GetLogsAsync(DateTime from, DateTime to);
        Task<string> GetPayloadAsync(string logId);
    }

    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _repository;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(IWeatherRepository repository, HttpClient httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("OpenWeatherApiKey");
        }

        public async Task FetchAndStoreWeatherDataAsync()
        {
            string city = "London"; // Hardcoded for now, you can enhance to handle multiple cities
            string requestUri = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    string blobName = $"{city}-{DateTime.UtcNow:yyyyMMddHHmmss}.json";
                    await _repository.SavePayloadToBlobAsync(blobName, responseBody);

                    var log = new WeatherLog
                    {
                        PartitionKey = city,
                        RowKey = Guid.NewGuid().ToString(),
                        Status = "Success",
                        BlobUri = blobName,
                        Timestamp = DateTimeOffset.UtcNow
                    };
                    await _repository.SaveLogAsync(log);
                }
                else
                {
                    await _repository.SaveLogAsync(new WeatherLog
                    {
                        PartitionKey = city,
                        RowKey = Guid.NewGuid().ToString(),
                        Status = "Failure",
                        Timestamp = DateTimeOffset.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                await _repository.SaveLogAsync(new WeatherLog
                {
                    PartitionKey = city,
                    RowKey = Guid.NewGuid().ToString(),
                    Status = "Error",
                    Timestamp = DateTimeOffset.UtcNow
                });
                throw;
            }
        }

        public async Task<IEnumerable<WeatherLog>> GetLogsAsync(DateTime from, DateTime to)
        {
            return await _repository.GetLogsByTimePeriodAsync(from, to);
        }

        public async Task<string> GetPayloadAsync(string logId)
        {
            return await _repository.GetPayloadFromBlobAsync(logId);
        }
    }

}
