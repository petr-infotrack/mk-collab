using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using AlertsAdmin.Domain.Enums;

namespace AlertsAdmin.Domain.Models
{
    public class MessageType
    {
        public int Id { get; set; }
        public string Template { get; set; }
        public AlertLevel Level { get; set; }
        public AlertPriority Priority { get; set; }
        public AlertNotification Notification { get; set; }
        public AlertStatus DefaultStatus { get; set; }

        public TimeSpan ExpiryTime { get; set; }
        public int ExpiryCount { get; set; }

        public IEnumerable<AlertInstance> Instances { get; set; }
    }
}
