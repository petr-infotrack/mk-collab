using AlertsAdmin.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertsAdmin.Domain.Models
{
    public class Alert
    {
        public int Id { get; set; }

        public IEnumerable<AlertInstance> Instances { get; set; }
        public AlertStatus Status { get; set; }
        public string StatusMessage { get; set; }
        public DateTime TimeStamp { get; set; }


        public int Count => Instances.Count();
        public AlertInstance FirstInstance => Instances.FirstOrDefault();
        public DateTime FirstTimestamp => Instances.FirstOrDefault().Timestamp;
        public AlertPriority AlertPriority => Instances.FirstOrDefault().MessageType.Priority;
    }
}
