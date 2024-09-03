using Azure.Data.Tables;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WeatherFunctionApp.Model.WeatherData;

namespace WeatherFunctionApp.Model
{
    public interface IWeatherRepository
    {
        Task SaveLogAsync(WeatherLog log);
        Task<IEnumerable<WeatherLog>> GetLogsByTimePeriodAsync(DateTime from, DateTime to);
        Task SavePayloadToBlobAsync(string blobName, string payload);
        Task<string> GetPayloadFromBlobAsync(string blobName);
    }

    public class WeatherRepository : IWeatherRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly TableServiceClient _tableServiceClient;

        public WeatherRepository(BlobServiceClient blobServiceClient, TableServiceClient tableServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _tableServiceClient = tableServiceClient;
        }

        public async Task SaveLogAsync(WeatherLog log)
        {
            var tableClient = _tableServiceClient.GetTableClient("WeatherLogs");
            await tableClient.CreateIfNotExistsAsync();
            await tableClient.AddEntityAsync(log);
        }

        public async Task<IEnumerable<WeatherLog>> GetLogsByTimePeriodAsync(DateTime from, DateTime to)
        {
            var tableClient = _tableServiceClient.GetTableClient("WeatherLogs");
            var logs = tableClient.QueryAsync<WeatherLog>(log => log.Timestamp >= from && log.Timestamp <= to);
            var result = new List<WeatherLog>();
            await foreach (var log in logs)
            {
                result.Add(log);
            }
            return result;
        }

        public async Task SavePayloadToBlobAsync(string blobName, string payload)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("weatherpayloads");
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(BinaryData.FromString(payload));
        }

        public async Task<string> GetPayloadFromBlobAsync(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("weatherpayloads");
            var blobClient = containerClient.GetBlobClient(blobName);
            var downloadInfo = await blobClient.DownloadAsync();
            using var reader = new StreamReader(downloadInfo.Value.Content);
            return await reader.ReadToEndAsync();
        }
    }

}
