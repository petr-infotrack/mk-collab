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

        public AlertInstanceRepository(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
            GenerateAlertInstances().Wait();
        }

        public async Task<IEnumerable<AlertInstance>> GetAllAlertInstancesAsync()
        {
            return await GetAlertInstancesAsync();
        }

        public async Task<IEnumerable<AlertInstance>> GetAlertInstancesAsync(Func<AlertInstance,bool> predicate = null)
        {
            return await Task.Run(() =>
                _alerts.Where(predicate ?? (a => true))
            );
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
