using AlertsAdmin.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IQueueRepository
    {
        Task<IEnumerable<KeyValuePair<string, int>>> GetQueueData();
    }
}
