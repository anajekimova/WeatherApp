using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherFunctionApp.Model
{
    internal class WeatherData
    {
        public class WeatherData
        {
            public string City { get; set; }
            public string Temperature { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public class WeatherLog
        {
            public string PartitionKey { get; set; } // e.g., City name
            public string RowKey { get; set; }       // Unique ID
            public string Status { get; set; }
            public string BlobUri { get; set; }
            public DateTimeOffset Timestamp { get; set; }
        }

    }
}
