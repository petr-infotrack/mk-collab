using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Domain.Enums;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Elastic.Models;
using Microsoft.EntityFrameworkCore;

namespace AlertsAdmin.Monitor.Logic
{
    public class AlertMonitoringRepository 
    {

        //REVIEW provisional compatibility with the logic of FE 
        private readonly Func<AlertMonitoringContext> _factory;

        //REVIEW provisional compatibility with the logic of FE
        private AlertMonitoringContext _db => _factory.Invoke();

        public AlertMonitoringRepository(Func<AlertMonitoringContext> factory)
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


        public async Task AddMessageAsync(ElasticErrorMessage message)
        {
            using (var context = _factory.Invoke())
            {
                
                //return (await Task.FromResult(
                //    context.Alerts
                //        .Include(a => a.Instances)
                //        .ThenInclude(ai => ai.MessageType)
                //        .Where(predicate ?? (a => true))
                //)).ToList();
            }
        }


    }


    public static class EqualityExtensions {

        public static bool IsEqual(this ElasticErrorMessage baseMessage, ElasticErrorMessage comparedMessage )
        {
            return baseMessage.MessageTemplate.Equals(comparedMessage.MessageTemplate, StringComparison.CurrentCultureIgnoreCase);
        }

    }
}
