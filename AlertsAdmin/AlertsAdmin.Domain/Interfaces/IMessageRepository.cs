using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<MessageType>> GetAllMessagesAsync();
        Task<IEnumerable<MessageType>> GetMessagesAsync(Func<MessageType, bool> predicate);
        Task<MessageType> GetAlertByIdAsync(int id);
        Task<IEnumerable<MessageType>> FindMessagesByMessage(string message);
    }
}
