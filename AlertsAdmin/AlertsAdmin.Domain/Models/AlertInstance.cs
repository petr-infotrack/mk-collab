using AlertsAdmin.Domain.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AlertsAdmin.Domain.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AlertInstance
    {
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public string ElasticId { get; set; }
        [JsonProperty]
        public DateTime Timestamp { get; set; }
        [JsonProperty]
        public string Message { get; set; }
        [JsonProperty]
        public string JsonData { get; set; }

        public int MessageTypeId { get; set; }
        public virtual MessageType MessageType { get; set; }

        public int? AlertId { get; set; }
        public Alert Alert { get; set; }
    }
}
