using WeatherApp.Model;

namespace WeatherApp.service
{
    public class WeatherFetchService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeatherFetchService> _logger;
        private readonly string _weatherApiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid=YOUR_API_KEY";

        public WeatherFetchService(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, ILogger<WeatherFetchService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cities = new List<(string Country, string City)>
                {
                    ("UK", "London"),
                    ("US", "New York"),
                    ("FR", "Paris")
                };

                    foreach (var (country, city) in cities)
                    {
                        var httpClient = _httpClientFactory.CreateClient();
                        var response = await httpClient.GetStringAsync(string.Format(_weatherApiUrl, city));
                        var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

                            var weatherData = new WeatherData
                            {
                                Country = country,
                                City = city,
                                Temperature = weatherResponse.Main.Temp,
                                LastUpdated = DateTime.UtcNow
                            };

                            dbContext.WeatherData.Add(weatherData);
                            await dbContext.SaveChangesAsync();
                        }
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching weather data.");
                }
            }
        }
    }

}
