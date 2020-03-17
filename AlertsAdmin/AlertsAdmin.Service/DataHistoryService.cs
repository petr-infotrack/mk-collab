using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Service.Models;
using Microsoft.Extensions.Configuration;

namespace AlertsAdmin.Service
{
    public class DataHistoryService
    {
        private readonly IQueueHistoryRepository _queueHistoryRepo;
        private readonly TimeSpan _bucketSize;

        public DataHistoryService(IQueueHistoryRepository queueHistoryRepository, IConfiguration config)
        {
            _queueHistoryRepo = queueHistoryRepository;
        }

        public async Task Process(IEnumerable<KeyValuePair<string,int>> data)
        {
            var queueData = ParseData(data);

            var latest = await _queueHistoryRepo.GetLastEntriesAsync(queueData.Select(q => q.Queue));

            var toBeUpdated = latest.Where(q => q.Timestamp - DateTime.Now > _bucketSize);
        }

        private IEnumerable<QueueData> ParseData(IEnumerable<KeyValuePair<string, int>> data)
        {
            return data.Select(d =>
                   new QueueData { 
                       Queue = d.Key, 
                       Count = d.Value}
                );
        }
    }
}
