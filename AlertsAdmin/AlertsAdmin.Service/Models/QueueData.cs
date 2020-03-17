using AlertsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Service.Models
{
    public class QueueData
    {
        public string Queue { get; set; }
        public int Count { get; set; }

        public static explicit operator QueueHistoryRecord(QueueData d) => 
            new QueueHistoryRecord {
                QueueName = d.Queue,
                Count = d.Count,
                Timestamp = DateTime.Now
            };
    }
}
