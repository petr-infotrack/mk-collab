using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlertsAdmin.Domain.Models
{
    public class AlertAcknowledgeRequest
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public TimeSpan AckTime { get; set; }
        public int? AckCount { get; set; }
    }
}
