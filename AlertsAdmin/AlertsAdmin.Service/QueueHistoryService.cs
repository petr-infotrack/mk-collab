using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AlertsAdmin.Service
{
    public class QueueHistoryService : IQueueHistoryService
    {
        private readonly IQueueHistoryRepository _queueHistoryRepo;
        private readonly ILogger<QueueHistoryService> _logger;
        private readonly int _bucketSize;

        public QueueHistoryService(IQueueHistoryRepository queueHistoryRepository, IConfiguration config, ILogger<QueueHistoryService> logger)
        {
            _queueHistoryRepo = queueHistoryRepository;
            _bucketSize = Int32.TryParse(config.GetValue<string>("QueueHistoryBucketSize"), out var size) ? size : 5;
            _logger = logger;
        }

        //TODO: Move to processing service so as not to block main thread.
        public async Task Process(IEnumerable<KeyValuePair<string,int>> data)
        {
            var queueData = ParseData(data);

            var toBeUpdated = new List<QueueHistoryRecord>();
            
            foreach(var queue in queueData)
            {
                try
                {
                    var latest = await _queueHistoryRepo.GetLastEntryAsync(queue.QueueName);
                    if (latest == null || DateTime.Now.Subtract(latest.Timestamp).TotalMinutes > _bucketSize)
                        toBeUpdated.Add(queue);
                }
                catch(Exception e)
                {
                    _logger.LogError($"Error evaluating queue ${queue.QueueName}\n{e.Message}");
                }
                
            }

            await _queueHistoryRepo.InsertRecords(toBeUpdated);
        }

        private IEnumerable<QueueHistoryRecord> ParseData(IEnumerable<KeyValuePair<string, int>> data)
        {
            return data.Select(d =>
                   new QueueHistoryRecord
                   {
                       QueueName = d.Key,
                       Count = d.Value,
                       Timestamp = DateTime.Now
                   }
                );
        }
    }
}
