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
        public int MessageTypeId { get; set; }
        public MessageType MessageType { get; set; }
        public AlertStatus Status { get; set; }
        public string StatusMessage { get; set; }
        public AlertPriority Priority { get; set; }
        public int InstanceCount { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? AckCount { get; set; }
        public TimeSpan? AckTime { get; set; }
        public int FirstInstanceId { get; set; }
        public AlertInstance FirstInstance { get; set; }
        public int LastInstanceId { get; set; }
        public AlertInstance LastInstance { get; set; }
        public bool Active { get; set; }

        public IEnumerable<AlertInstance> Instances { get; set; }

        public string FirstOccuranceString => FirstInstance.Timestamp.ToShortTimeString();
        public string LastOccuranceString => LastInstance.Timestamp.ToShortTimeString();

        //public int Count => Instances.Count();
        //public AlertInstance FirstInstance => Instances.FirstOrDefault();

        //public string FirstOccuranceString => FirstOccurance.ToShortTimeString();

        //public string LastOccuranceString => LastOccurance.ToShortTimeString();
        //public AlertPriority AlertPriority => Instances.FirstOrDefault().MessageType.Priority;
        //public string Template => Instances.FirstOrDefault().MessageType.Template;
        //public int MessageId => Instances.FirstOrDefault().MessageType.Id;
    }
}
