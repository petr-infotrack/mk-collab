﻿using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;

namespace AlertsAdmin.Data
{
    public class AlertRepository : IAlertRepository
    {
        private static IEnumerable<MessageType> _alets = new List<MessageType>
        {
            new MessageType{Id=1,Template= "THIS IS ALERT 1"},
            new MessageType{Id=2,Template= "THIS IS ALERT 2"},
            new MessageType{Id=3,Template= "GOTCHA"},
            new MessageType{Id=4,Template= "THIS IS ALERT 4"},
            new MessageType{Id=5,Template= "GOT A TEST?"},
            new MessageType{Id=6,Template= "THIS IS ALERT 6"},
        };

        public async Task<IEnumerable<MessageType>> GetAllAlertsAsync()
        {
            return await GetAlertsAsync(x => true);
        }

        public async Task<MessageType> GetAlertByIdAsync(int id)
        {
            var alert = await GetAlertsAsync(x => x.Id == id);
            return alert.Single();
        }

        public async Task<IEnumerable<MessageType>> FindAlertsByMessage(string message)
        {
            var alerts = await GetAlertsAsync(x => x.Template.ToUpper().Contains(message.ToUpper()));
            return alerts;
        }

        public async Task<IEnumerable<MessageType>> GetAlertsAsync(Func<MessageType, bool> predicate = null)
        {
            return await Task.Run(() =>
                _alets.Where(predicate ?? (s => true)).ToList()
            );
        }
    }
}
