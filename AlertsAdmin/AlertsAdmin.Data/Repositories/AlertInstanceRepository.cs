using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertsAdmin.Data.Repositories
{
    public class AlertInstanceRepository : IAlertInstanceRepository
    {
        private AlertMonitoringContext _db => _factory.Invoke();
        private readonly Func<AlertMonitoringContext> _factory;

        public AlertInstanceRepository(Func<AlertMonitoringContext> factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<AlertInstance>> GetAlertInstancesAsync(int alertId)
        {
            return await GetAlertInstancesAsync(a => a.AlertId == alertId);
        }

        public async Task<IEnumerable<AlertInstance>> GetAllAlertInstancesAsync()
        {
            return await GetAlertInstancesAsync();
        }

        public async Task<IEnumerable<AlertInstance>> GetAlertInstancesAsync(Func<AlertInstance, bool> predicate = null)
        {
            using (var context = _db)
            {
                return (await Task.FromResult(
                    context.AlertInstances
                        .Where(predicate ?? (a => true))
                )).ToList();
            }
        }
    }
}
