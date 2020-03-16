using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IMessageSearch
    {
        Task<IEnumerable<MessageType>> Search(MessageSearchOptions options, IEnumerable<MessageType> messages);
        Task<IEnumerable<MessageType>> Search(MessageSearchOptions options);
    }
}
