using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using AlertsAdmin.Data.Contexts;

namespace AlertsAdmin.Data.Repositories
{
    public class AlertInstanceRepository : IAlertInstanceRepository
    {
        private readonly Func<AlertMonitoringContext> _factory;
        private AlertMonitoringContext _db => _factory.Invoke();


        public AlertInstanceRepository(Func<AlertMonitoringContext> factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<AlertInstance>> GetAllAlertInstancesAsync()
        {
            return await GetAlertInstancesAsync();
        }

        public async Task<IEnumerable<AlertInstance>> GetMessageInstances(MessageType messageType)
        {
            return await GetMessageInstances(messageType.Id);
        }

        public async Task<IEnumerable<AlertInstance>> GetMessageInstances(int messageTypeId)
        {
            return await GetAlertInstancesAsync(a => a.MessageType.Id == messageTypeId);
        }

        public async Task<IEnumerable<AlertInstance>> GetAlertInstancesAsync(Func<AlertInstance, bool> predicate = null)
        {
            using (var context = _db)
            {
                return (await Task.FromResult(context.AlertInstances.Where(predicate ?? (a => true)))).ToList();
            }
        }
    }
}
