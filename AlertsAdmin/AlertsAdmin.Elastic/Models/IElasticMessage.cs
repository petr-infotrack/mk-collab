namespace AlertsAdmin.Elastic.Models
{
    using System;

    public interface IElasticMessage
    {
        string ElasticId { get; set; }

        string Message { get; set; }

        DateTime Timestamp { get; set; }
    }
}
