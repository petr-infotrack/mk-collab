using System.Collections.Generic;
using System.Text;
using Nest;

namespace AlertsAdmin.Elastic
{
    public class Fields
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
