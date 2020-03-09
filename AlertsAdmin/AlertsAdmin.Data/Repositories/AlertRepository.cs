using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Enums;
using AlertsAdmin.Domain.Models;
using System.Threading.Tasks;
using System.Linq;
using AlertsAdmin.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AlertsAdmin.Data.Repositories
{
    public class AlertRepository : IAlertRepository
    {

        private AlertMonitoringContext _db => _factory.Invoke();
        private readonly Func<AlertMonitoringContext> _factory;

        public AlertRepository(Func<AlertMonitoringContext> factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            return await GetAlertsAsync();
        }

        public async Task<IEnumerable<Alert>> GetActiveAlertsAsync()
        {
            return await GetAlertsAsync(a => a.Status != AlertStatus.Disabled && a.Status != AlertStatus.Acknowladged);
        }

        public async Task<IEnumerable<Alert>> GetAlertsAsync(Func<Alert, bool> predicate = null)
        {
            using (var context = _db)
            {
                return (await Task.FromResult(
                    context.Alerts
                        .Include(a => a.Instances)
                            .ThenInclude(ai => ai.MessageType)
                        .Where(predicate ?? (a => true))
                )).ToList();
            }
        }

    }
}
