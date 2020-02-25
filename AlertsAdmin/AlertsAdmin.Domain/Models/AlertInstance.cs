using AlertsAdmin.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Domain.Models
{
    public class AlertInstance
    {
        public int Id { get; set; }
        public string ElasticId { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageType MessageType { get; set; }
        public string Message { get; set; }
        public string JsonData { get; set; }
    }
}
