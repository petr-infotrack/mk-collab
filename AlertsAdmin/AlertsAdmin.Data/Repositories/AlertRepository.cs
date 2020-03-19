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

        public async Task AcknowledgeAlert(AlertAcknowledgeRequest request)
        {
            using(var context = _db)
            {
                var alert = await context.Alerts.FindAsync(request.Id);
                alert.Status = AlertStatus.Acknowladged;
                alert.StatusMessage = request.Message;
                if (request.AckCount != null)
                    alert.AckCount = request.AckCount;
                if (request.AckTime != null)
                    alert.AckTime = request.AckTime;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            return await GetAlertsAsync();
        }

        public async Task<IEnumerable<Alert>> GetActiveAlertsAsync()
        {


            var data =  await GetAlertsAsync(a => a.Status != AlertStatus.Disabled && a.Status != AlertStatus.Acknowladged);

            return data.ToList();
        }

        public async Task<Alert> GetAlertAsync(int id)
        {
            return (await GetAlertsAsync(a => a.Id == id)).Single();
        }

        public async Task<IEnumerable<Alert>> GetAlertsAsync(Func<Alert, bool> predicate = null)
        {
            using (var context = _db)
            {
                var data =  context.Alerts.Include(x => x.MessageType)
                    .Where(predicate ?? (a => true)).ToList();

                return await Task.FromResult(data);
            }
        }

    }
}
