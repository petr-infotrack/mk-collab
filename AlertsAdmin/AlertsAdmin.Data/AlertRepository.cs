using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using System.Threading.Tasks;
using System.Linq;

namespace AlertsAdmin.Data
{
    public class AlertRepository : IAlertRepository
    {
        private IEnumerable<Alert> _alerts;
        private readonly IAlertInstanceRepository _alertInstanceRepository;

        public AlertRepository(IAlertInstanceRepository alertInstanceRepository)
        {
            _alertInstanceRepository = alertInstanceRepository;
            GenerateAlerts().Wait();
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            return await GetAlertsAsync();
        }

        public async Task<IEnumerable<Alert>> GetAlertsAsync(Func<Alert,bool> predicate = null)
        {
            return await Task.Run(() =>
            {
                return _alerts.Where(predicate ?? (a => true));
            });
        }

        private async Task GenerateAlerts()
        {
            var alerts = new List<Alert>();
            var instances = (await _alertInstanceRepository.GetAllAlertInstancesAsync()).ToList();
            var grouped = instances
                            .GroupBy(i => i.MessageType, i =>
                            {
                                i.MessageType = i.MessageType;
                                return i;
                            });
            int i = 1;
            foreach(var instanceType in grouped)
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

            _alerts = alerts;
        }
    }
}
