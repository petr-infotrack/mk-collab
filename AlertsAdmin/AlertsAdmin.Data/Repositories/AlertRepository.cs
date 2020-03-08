using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using System.Threading.Tasks;
using System.Linq;
using AlertsAdmin.Data.Contexts;

namespace AlertsAdmin.Data.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly IAlertInstanceRepository _alertInstanceRepository;


        private AlertMonitoringContext _db => _factory.Invoke();
        private readonly Func<AlertMonitoringContext> _factory;

        public AlertRepository(IAlertInstanceRepository alertInstanceRepository, Func<AlertMonitoringContext> factory)
        {
            _alertInstanceRepository = alertInstanceRepository;
            _factory = factory;
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            return await GetAlertsAsync();
        }

        public async Task<IEnumerable<Alert>> GetAlertsAsync(Func<Alert, bool> predicate = null)
        {
            using (var context = _db)
            {
                return (await Task.FromResult(context.Alerts.Where(predicate ?? (a => true)))).ToList();
            }
        }

        private async Task<IEnumerable<Alert>> GroupAlertInstancesAsync()
        {
            var instances = await _alertInstanceRepository.GetAllAlertInstancesAsync();
            return await GroupAlertInstancesAsync(instances);
        }

        private async Task<IEnumerable<Alert>> GroupAlertInstancesAsync(IEnumerable<AlertInstance> alertInstances)
        {
            return await Task.Run(() =>
            {
                var alerts = new List<Alert>();
                var grouped = alertInstances
                                .GroupBy(i => i.MessageType, i =>
                                {
                                    i.MessageType = i.MessageType;
                                    return i;
                                });

                int i = 1;
                foreach (var instanceType in grouped)
                {
                    alerts.Add(
                            new Alert()
                            {
                                Id = i,
                                Instances = instanceType.AsEnumerable()
                            }
                        );
                    i++;
                }

                return alerts;
            });
        }
    }
}
