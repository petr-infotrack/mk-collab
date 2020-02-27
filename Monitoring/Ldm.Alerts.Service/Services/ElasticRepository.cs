using Ldm.Alerts.Service.Services.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace Ldm.Charting.Data
{

    public class ElasticDataRepository : IElasticDataRepository
    {
        private static readonly HashSet<string> EligibleEnvironements = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            "Production",
            "Live",
            "Production-AU",
            "Prod"
        };

        public List<ErrorOcccurences> GetErrorsByContentFilter(string filter, int maxScanPeriod)
        {
            var error = this.GetErrorsByScanRange(maxScanPeriod);

            var filtered = error.Where(c => c.ErrorMessage.Contains(filter));

            return filtered.ToList();
        }

        protected List<ErrorOcccurences> GetErrorsByScanRange(int maxScanPeriod)
        {
            List<ErrorOcccurences> response = null;
            
            var searchResponse = ElasticClientSingleton.Instance.Search<ErrorOcccurences>(x => x
                    .Index(Indices.Index(new[]
                    {
                        $"*-{DateTime.Now.ToString("yyyy.MM.dd")}",
                        $"*-{DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd")}"
                    }))
                    .From(0)
                    .Size(2000)
                    .Query(q => q.Bool(m => m.Must(e =>
                        e.Match(r => r.Field(f => f.Level).Query("Error"))
                        && e.DateRange(r => r.Field(f => f.Timestamp)
                            .GreaterThanOrEquals(DateTime.Now.AddMinutes(maxScanPeriod * -1))
                            .LessThanOrEquals(DateTime.Now)
                        )
                    )))
            );

            if (searchResponse != null && searchResponse.Documents != null)
            {
                response = searchResponse.Documents.ToList();   
            }

            // Not sure if this is necessary but there's potentially a lot of data here...
            searchResponse = null;
            ElasticClientSingleton.Instance.ClearCache("*");

            if (response == null)
            {
                response = new List<ErrorOcccurences>();
            }

            return response;
        }

    
        private string GetErrorMessage(IGrouping<string, ErrorOcccurences> group)
        {
            var sb = new StringBuilder();

            var application = group.Select(g => g.Fields.Application).Distinct();
            if (application.Any())
            {
                sb.Append(string.Join(", ", application));
            }

            var orderId = group.Max(g => g.Fields.OrderIdStr);
            if (string.IsNullOrWhiteSpace(orderId))
            {
                orderId = group.Max(g => g.Fields.OrderIdInt).ToString();
            }
            if (!string.IsNullOrWhiteSpace(orderId) && orderId != "0")
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.AppendFormat("({0})", orderId);
            }

            var regions = group.Select(g => g.Fields.Region).Where(r => !string.IsNullOrWhiteSpace(r)).Distinct();
            if (regions.Any())
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(string.Join(", ", regions));
            }

            if (sb.Length > 0) 
            {
                sb.AppendFormat(": {0}", group.First().ErrorMessage);
                return sb.ToString();
            }
            else
            {
                return group.First().ErrorMessage;
            }
        }

    }
}
