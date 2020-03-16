using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IQueueHistoryRepository
    {
        Task<IEnumerable<QueueHistoryRecord>> GetQueueHistoryAsync(Func<QueueHistoryRecord, bool> predicate);
        Task<IEnumerable<QueueHistoryRecord>> GetLastEntriesAsync(IEnumerable<string> Queues);
    }
}
