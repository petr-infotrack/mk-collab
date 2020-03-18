using AlertsAdmin.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AlertsAdmin.Domain.Models
{
    public class AlertInstance
    {
        public int Id { get; set; }
        public string ElasticId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string JsonData { get; set; }

        public int MessageTypeId { get; set; }
        public virtual MessageType MessageType { get; set; }

        public int? AlertId { get; set; }
        public Alert Alert { get; set; }
    }
}
