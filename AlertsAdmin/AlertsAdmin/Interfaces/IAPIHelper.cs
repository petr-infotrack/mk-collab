using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlertsAdmin.Interfaces
{
    public interface IAPIHelper
    {
        Task<Alert> GetAlertAsync(int id);
        Task<MessageType> GetMessageAsync(int id);
    }

}
