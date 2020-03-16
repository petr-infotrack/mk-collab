using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Models;
using AlertsAdmin.Service.Interfaces;

namespace AlertsAdmin.Service.Search
{
    public class MessageEnumCriteria<T> : IMessageCriteria
    {
        private static IEnumerable<T> _states;

        public MessageEnumCriteria(IEnumerable<T> states)
        {
            _states = states;
        }

        public async Task<IEnumerable<MessageType>> Match(IEnumerable<MessageType> messages)
        {
            var prop = GetProperty();
            return await Task.Run(() =>
                messages.Where(m => Match(prop,m))
            );
        }

        private PropertyInfo GetProperty()
        {
            var props = typeof(MessageType).GetProperties();
            foreach (var p in props)
            {
                if (p.PropertyType.IsAssignableFrom(typeof(T)))
                    return  p;
            }
            throw new ArgumentException($"{typeof(T).Name} not a memeber of MessageType");
        }

        private bool Match(PropertyInfo property, MessageType message)
        {
            var p = message.GetType().GetProperty(property.Name);
            var val = p.GetValue(message);
            if (_states.Contains((T)val))
                return true; 
            return false;
        }
    }
}
