using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Service.Interfaces;

namespace AlertsAdmin.Service.Search
{
    public class MessageTemplateCriteria : IMessageCriteria
    {
        private static string _searchString;

        public MessageTemplateCriteria(string searchString)
        {
            _searchString = searchString;
        }

        public async Task<IEnumerable<MessageType>> Match(IEnumerable<MessageType> messages)
        {
            return await Task.Run(() =>
                messages.Where(m => m.Template.ToUpper().Contains(_searchString.ToUpper()))
            );
        }
    }
}
