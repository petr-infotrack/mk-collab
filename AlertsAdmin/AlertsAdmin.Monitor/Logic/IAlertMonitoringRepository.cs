using AlertsAdmin.Domain.Models;
using AlertsAdmin.Elastic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlertsAdmin.Monitor.Logic
{
    public interface IAlertMonitoringRepository
    {
        Task AddMessageAsync(ElasticErrorMessage message);

        Task<IEnumerable<Alert>> GetActiveAlertsAsync();

        Task<IEnumerable<Alert>> GetAlertsAsync(Func<Alert, bool> predicate = null);

        Task<IEnumerable<Alert>> GetAllAlertsAsync();

        Task<int> SaveAsync();
    }
}