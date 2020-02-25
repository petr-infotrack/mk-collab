using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Enums;

namespace AlertsAdmin.Domain.Models
{
    public class MessageSearchOptions
    {
        public string MessageTemplate { get; set; }
        public IEnumerable<AlertLevel> Levels { get; set; }
        public IEnumerable<AlertPriority> Priorities { get; set; }
        public IEnumerable<AlertNotification> Notifications { get; set; }
        public IEnumerable<AlertStatus> DefaultStatuses { get; set; }
    }
}
