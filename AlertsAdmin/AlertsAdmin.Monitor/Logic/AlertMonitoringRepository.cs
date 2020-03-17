using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Domain.Enums;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Elastic.Models;
using Microsoft.EntityFrameworkCore;
using AlertsAdmin.Monitor.Logic.Mappers;

namespace AlertsAdmin.Monitor.Logic
{
    public class AlertMonitoringRepository
    {

        //REVIEW provisional compatibility with the logic of FE 
        private readonly Func<AlertMonitoringContext> _factory;

        //REVIEW provisional compatibility with the logic of FE
        private readonly AlertMonitoringContext _db; // => _factory.Invoke();

        public AlertMonitoringRepository(Func<AlertMonitoringContext> factory)
        {
            _factory = factory;

            _db = factory.Invoke();
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
                return (await Task.FromResult(
                    _db.Alerts
                        .Include(a => a.Instances)
                        .ThenInclude(ai => ai.MessageType)
                        .Where(predicate ?? (a => true))
                )).ToList();
        }


        public async Task AddMessageAsync(ElasticErrorMessage message)
        {
            //TODO -- fix cultural comparison or implement hash compare
            var messageType = await _db.MessageTypes.FirstOrDefaultAsync(x => x.Template !=null && x.Template ==  message.MessageTemplate);

            if (messageType == null)
            {
                messageType = new MessageTypeMapper().Map(message);

                await _db.MessageTypes.AddAsync(messageType);
            }

            AlertInstance instance = new AlertInstanceMapper().Map(message, messageType);

            var firstInstance = await _db.AlertInstances.FirstOrDefaultAsync((x => x.MessageTypeId == messageType.Id));
            if (firstInstance == null)
            {
                firstInstance = instance;
            }
            

            // TODO Clarify aggregation logic
            var alert = await _db.Alerts.FirstOrDefaultAsync(x =>
                x.MessageType != null && x.MessageType.Template == message.MessageTemplate);

            if (alert == null)
            {
                alert = new Alert()
                {
                    Status = messageType.DefaultStatus,
                    StatusMessage = null,
                    TimeStamp = DateTime.Now,
                    MessageType = messageType,
                    FirstInstance = firstInstance,
                    LastInstance = instance
                };

                _db.Alerts.Add(alert);
            }
            
            await _db.AlertInstances.AddAsync(instance);
        }

        public async Task AddMessageCollectionAsync(IEnumerable<ElasticErrorMessage> messages)
        {
            //TODO -- fix cultural comparison or implement hash compare
            //var messageType = await _db.MessageTypes.FirstOrDefaultAsync(x => x.Template != null && x.Template == message.MessageTemplate);

            //if (messageType == null)
            //{
            //    messageType = new MessageTypeMapper().Map(message);

            //    await _db.MessageTypes.AddAsync(messageType);
            //}

            //// TODO Clarify aggregation logic
            ////var alert = await _db.Alerts.FirstOrDefaultAsync(x =>
            ////    x.Template != null && x.Template == message.MessageTemplate);

            ////if (alert == null)
            ////{
            ////    alert = new Alert()
            ////    {
            ////        T
            ////    };

            ////    _db.Alerts.Add(alert);
            ////}

            //AlertInstance instance = new AlertInstanceMapper().Map(message, messageType);

            //await _db.AlertInstances.AddAsync(instance);
        }


        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }

    }
}