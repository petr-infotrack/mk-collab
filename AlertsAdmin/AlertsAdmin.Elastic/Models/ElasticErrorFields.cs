using Nest;

namespace AlertsAdmin.Elastic.Models
{
    public class ElasticErrorFields
    {
        public string MachineName { get; set; }

        public string Application { get; set; }

        [Text(Name = "orderid")]
        public string OrderIdStr { get; set; }

        [Text(Name = "OrderId")]
        public int OrderIdInt { get; set; }

        public string Environment { get; set; }

        public string Region { get; set; }
    }
}
