using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlertsAdmin.Models
{
    public class MessageViewModel
    {
        public IEnumerable<MessageType> Messages { get; set; }
        public MessageSearchOptions options { get; set; }
    }
}
