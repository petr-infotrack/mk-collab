using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AlertsAdmin.Data
{
    public class AlertInstanceRepository : IAlertInstanceRepository
    {
        private IEnumerable<AlertInstance> _alerts;
        private readonly IMessageRepository _messageRepo;

        private readonly AlertMonitoringContext _db;


        public AlertInstanceRepository(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
#if !SIMULATION
            //TODO -- once structures completed, it'll be replaced with injection
            _db = new AlertMonitoringContext();
#else
            GenerateAlertInstances().Wait();
#endif
        }

        public async Task<IEnumerable<AlertInstance>> GetAllAlertInstancesAsync()
        {
            return await GetAlertInstancesAsync();
        }

        public async Task<IEnumerable<AlertInstance>> GetAlertInstancesAsync(Func<AlertInstance,bool> predicate = null)
        {

#if !SIMULATION

            return await Task.FromResult(_db.AlertInstances.Where(predicate ?? (a => true)));

#else
            return await Task.Run(() =>
                _alerts.Where(predicate ?? (a => true))
            );
#endif
        }

        private async Task GenerateAlertInstances()
        {
            var alerts = new List<AlertInstance>();
            var messages = (await _messageRepo.GetAllMessagesAsync()).ToList();
            var random = new Random();
            int randomId;
            for(int i=1; i <=50; i++)
            {
                randomId = random.Next(1, messages.Count);
                alerts.Add(
                        new AlertInstance()
                        {
                            Id = i,
                            MessageType = messages.Where(m => m.Id == randomId).Single()
                        }
                    );
            }
            _alerts = alerts;
        }
    }
}
