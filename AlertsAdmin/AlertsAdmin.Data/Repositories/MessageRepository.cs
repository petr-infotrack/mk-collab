using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Domain.Enums;
using AlertsAdmin.Data.Contexts;

namespace AlertsAdmin.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly Func<AlertMonitoringContext> _factory;
        private AlertMonitoringContext _db => _factory.Invoke();


        public MessageRepository(Func<AlertMonitoringContext> factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<MessageType>> GetAllMessagesAsync()
        {
            return await GetMessagesAsync();
        }

        public async Task<MessageType> GetMessageByIdAsync(int id)
        {
            var messages = await GetMessagesAsync(x => x.Id == id);
            if(messages.Count() > 1)
                throw new ArgumentOutOfRangeException($"Multiple messages returned for id: {id}");
            if (messages.Count() == 1)
                return messages.Single();
            return null;
        }

        public async Task<IEnumerable<MessageType>> FindMessagesByMessageAsync(string message)
        {
            var messages = await GetMessagesAsync(x => x.Template.ToUpper().Contains(message.ToUpper()));
            return messages;
        }

        public async Task<IEnumerable<MessageType>> GetMessagesAsync(Func<MessageType, bool> predicate = null)
        {
            using(var context = _db)
            {
                return (await Task.FromResult(context.MessageTypes.Where(predicate ?? (a => true)))).ToList();
            }
        }

        public async Task UpdateMessageAsync(MessageType message)
        {
            using(var context = _db)
            {
                context.Update(message);
                await context.SaveChangesAsync();
            }
        }
    }
}
