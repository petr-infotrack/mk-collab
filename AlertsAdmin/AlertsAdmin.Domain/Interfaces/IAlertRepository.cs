using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Models;

namespace AlertsAdmin.Domain.Interfaces
{
    public interface IAlertRepository
    {
        Task<IEnumerable<Alert>> GetAllAlertsAsync();
        Task<IEnumerable<Alert>> GetAlertsAsync(Func<Alert, bool> predicate);
        Task<IEnumerable<Alert>> GetActiveAlertsAsync();
        Task<Alert> GetAlertAsync(int id);
    }
}
