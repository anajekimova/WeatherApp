using Microsoft.AspNetCore.Mvc;
using WeatherApp.Model;

namespace WeatherApp.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherDbContext _dbContext;

        public WeatherController(WeatherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("min-max")]
        public IActionResult GetMinMaxTemperatures()
        {
            var result = _dbContext.WeatherData
                .GroupBy(w => new { w.Country, w.City })
                .Select(g => new
                {
                    g.Key.Country,
                    g.Key.City,
                    MinTemperature = g.Min(x => x.Temperature),
                    MaxTemperature = g.Max(x => x.Temperature),
                    LastUpdateTime = g.Max(x => x.LastUpdated)
                }).ToList();

            return Ok(result);
        }
    }

}
