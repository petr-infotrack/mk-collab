using AlertsAdmin.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AlertsAdmin.Domain.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public AlertStatus Status { get; set; }
        public string StatusMessage { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? AckCount { get; set; }
        public TimeSpan? AckTime { get; set; }


        public IEnumerable<AlertInstance> Instances { get; set; }

        public int Count => Instances.Count();
        public AlertInstance FirstInstance => Instances.FirstOrDefault();
        [DisplayName("First Occurance")]
        public DateTime FirstOccurance => Instances.FirstOrDefault().Timestamp;
        public string FirstOccuranceString => FirstOccurance.ToShortTimeString();
        [DisplayName("Last Occurance")]
        public DateTime LastOccurance => Instances.LastOrDefault().Timestamp;
        public string LastOccuranceString => LastOccurance.ToShortTimeString();
        public AlertPriority AlertPriority => Instances.FirstOrDefault().MessageType.Priority;
        public string Template => Instances.FirstOrDefault().MessageType.Template;
        public int MessageId => Instances.FirstOrDefault().MessageType.Id;
    }
}
