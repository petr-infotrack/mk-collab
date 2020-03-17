using System;
using Nest;

namespace AlertsAdmin.Elastic.Models
{
    [ElasticsearchType(RelationName = "logevent")]
    public class ElasticErrorMessage
    {
        [Text(Name = "@timestamp")]
        public DateTime Timestamp { get; set; }

        public string ElasticId { get; set; }

        [Text(Name = "fields")]
        public ElasticErrorFields Fields { get; set; }

        [Text(Name = "messageTemplate")]
        public string MessageTemplate { get; set; }

        [Text(Name = "message")]
        public string Message { get; set; }

        [Text(Name = "level")]
        public string Level { get; set; }
    }
}
