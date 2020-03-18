using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IQueueHistoryService
    {
        Task Process(IEnumerable<KeyValuePair<string, int>> data);
    }
}
