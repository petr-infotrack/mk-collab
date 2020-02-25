using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlertsAdmin.Models
{
    public class AlertViewModel
    {
        public IEnumerable<MessageType> Alerts { get; set; }
        public string SearchString { get; set; }
    }
}
