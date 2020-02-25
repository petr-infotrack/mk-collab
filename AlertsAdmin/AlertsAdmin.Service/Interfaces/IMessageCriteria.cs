using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Service.Interfaces
{
    public interface IMessageCriteria
    {
        Task<IEnumerable<MessageType>> Match(IEnumerable<MessageType> messages);
    }
}
