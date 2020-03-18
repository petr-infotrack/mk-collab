﻿using System;
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
                await SaveAsync();
            }

            AlertInstance instance = new AlertInstanceMapper().Map(message, messageType);

            await _db.AlertInstances.AddAsync(instance);
            await SaveAsync();


            var firstInstance = await _db.AlertInstances.Where(x => x.MessageTypeId == messageType.Id)
                                    .OrderBy(o => o.Timestamp)
                                    .FirstOrDefaultAsync()
                                ?? instance;


            // TODO Clarify aggregation logic
            var alert = await _db.Alerts.OrderBy(o => o.TimeStamp).FirstOrDefaultAsync(x =>
                x.MessageType != null && x.MessageType.Template == message.MessageTemplate);

            if (alert == null)
            {
                alert = new AlertMapper().Map(messageType, firstInstance.Id, instance.Id);

                _db.Alerts.Add(alert);

                await SaveAsync();
            }

            //instance.AlertId = alert.Id;
            instance.Alert = alert;
           
            _db.Entry(alert).State = EntityState.Modified;
            _db.Entry(instance).State = EntityState.Modified;

            await SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }

    }
}