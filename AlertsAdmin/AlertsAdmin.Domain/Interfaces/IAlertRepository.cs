using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IAlertRepository
    {
        public Task<IEnumerable<MessageType>> GetAllAlertsAsync();
        public Task<IEnumerable<MessageType>> GetAlertsAsync(Func<MessageType, bool> predicate);
        public Task<MessageType> GetAlertByIdAsync(int id);
        public Task<IEnumerable<MessageType>> FindAlertsByMessage(string message);
    }
}
