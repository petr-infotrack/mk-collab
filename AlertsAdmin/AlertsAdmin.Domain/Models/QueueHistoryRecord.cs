using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Domain.Models
{
    public class QueueHistoryRecord
    {
        public int Id { get; set; }
        public string QueueName { get; set; }
        public int Count { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
