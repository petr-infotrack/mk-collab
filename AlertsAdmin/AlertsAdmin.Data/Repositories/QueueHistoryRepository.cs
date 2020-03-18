using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Logging;

namespace AlertsAdmin.Data.Repositories
{
    public class QueueHistoryRepository: IQueueHistoryRepository
    {
        private readonly Func<AlertMonitoringContext> _factory;
        private readonly ILogger<QueueHistoryRepository> _logger;
        private AlertMonitoringContext _db => _factory.Invoke();

        public QueueHistoryRepository(Func<AlertMonitoringContext> factory, ILogger<QueueHistoryRepository> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task InsertRecords(IEnumerable<QueueHistoryRecord> data)
        {
            try
            {
                using var context = _db;
                context.QueueHistory.AddRange(data);
                await context.SaveChangesAsync();
            }catch(Exception e)
            {
                _logger.LogError($"Could not insert queue history records: {e.Message}");
            }
        }


        public async Task<IEnumerable<QueueHistoryRecord>> GetLastEntriesAsync(IEnumerable<string> Queues)
        {
            return await Task.WhenAll(
                    Queues.Select(async (q) => await GetLastEntryAsync(q)));
        }

        public async Task<QueueHistoryRecord> GetLastEntryAsync(string Queue)
        {
            try
            {
                var records = await GetQueueHistoryAsync(q => q.QueueName.ToUpper() == Queue.ToUpper());
                return records.OrderBy(r => r.Timestamp).LastOrDefault();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IEnumerable<QueueHistoryRecord>> GetQueueHistoriesAsync(IEnumerable<string> Queues)
        {
            return await GetQueueHistoryAsync(q => Queues.ToList().ConvertAll(q => q.ToUpper()).Contains(q.QueueName.ToUpper()));
        }

        public async Task<IEnumerable<QueueHistoryRecord>> GetQueueHistoryAsync(Func<QueueHistoryRecord,bool> predicate = null)
        {
            using var context = _db;
            return (await Task.FromResult(context.QueueHistory.Where(predicate ?? (a => true)))).ToList();
        }
    }
}
