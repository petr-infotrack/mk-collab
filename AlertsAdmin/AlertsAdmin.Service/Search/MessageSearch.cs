using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Service.Interfaces;
using AlertsAdmin.Domain.Enums;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Interfaces;

namespace AlertsAdmin.Service.Search
{
    public class MessageSearch : IMessageSearch
    {
        private static IAlertRepository _messageRepo;
        public MessageSearch(IAlertRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }

        public async Task<IEnumerable<MessageType>> Search(MessageSearchOptions options, IEnumerable<MessageType> messages)
        {
            var criteria = new List<IMessageCriteria>();
            if (!string.IsNullOrEmpty(options.MessageTemplate))
            {
                criteria.Add(new MessageTemplateCriteria(options.MessageTemplate));
            }
            if (options.Levels != null)
            {
                criteria.Add(new MessageEnumCriteria<AlertLevel>(options.Levels));
            }
            if(options.Priorities != null)
            {
                criteria.Add(new MessageEnumCriteria<AlertPriority>(options.Priorities));
            }
            if(options.Notifications != null)
            {
                criteria.Add(new MessageEnumCriteria<AlertNotification>(options.Notifications));
            }
            if(options.DefaultStatuses != null)
            {
                criteria.Add(new MessageEnumCriteria<AlertStatus>(options.DefaultStatuses));
            }

            return await new AndMessageCriteria(criteria).Match(messages);
        }

        public async Task<IEnumerable<MessageType>> Search(MessageSearchOptions options)
        {
            var messages = await _messageRepo.GetAllAlertsAsync();
            return await Search(options, messages);
        }

    }
}
