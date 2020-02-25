using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Service.Interfaces;

namespace AlertsAdmin.Service.Search
{
    public class AndMessageCriteria : IMessageCriteria
    {
        private static IEnumerable<IMessageCriteria> _criteria;
        public AndMessageCriteria(IEnumerable<IMessageCriteria> criteria)
        {
            _criteria = criteria;
        }

        public async Task<IEnumerable<MessageType>> Match(IEnumerable<MessageType> messages)
        {
            return await Task.Run(async () =>
            {
                foreach(var c in _criteria)
                {
                    messages = await c.Match(messages);
                }
                return messages;
            });
        }
    }
}
