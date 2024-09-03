using Microsoft.EntityFrameworkCore;

namespace WeatherApp.Model
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

        public DbSet<WeatherData> WeatherData { get; set; }
    }

}
