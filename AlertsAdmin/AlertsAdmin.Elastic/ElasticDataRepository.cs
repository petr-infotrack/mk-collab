using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertsAdmin.Elastic.Models;
using Nest;

namespace AlertsAdmin.Elastic
{
    public interface ISourceRepository<T>
    {
        List<T> GetErrorMessages(DateTime scanDateTime, int scanRangeMin, int maxScanPeriod);
    }

    public class ElasticDataRepository : ISourceRepository<ElasticErrorMessage>
    {
        private int DEFAULT_OFFSET_HOURS = 0;
        private int DEFAULT_SCAN_INTERVAL_MINUTES = 10;

        private (string endpoint, string user, string pwd, int scanInterval) _config;

        public void Configure((string endpoint, string user, string pwd, int scanInterval) config)
        {
            _config = config;
        }

        private static readonly HashSet<string> EligibleEnvironements =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Production",
                "Live",
                "Production-AU",
                "Prod"
            };


        public List<ElasticErrorMessage> GetErrorMessages(DateTime scanDateTime, int scanRangeMin, int maxScanPeriod)
        {
            var indexRange = new[]
            {
                $"*-{scanDateTime:yyyy.MM.dd}",
                $"*-{scanDateTime.AddDays(-1):yyyy.MM.dd}"
            };

            var startDateTime = scanDateTime.AddHours(DEFAULT_OFFSET_HOURS).AddMinutes(scanRangeMin * -1);
            var endDateTime = scanDateTime.AddHours(DEFAULT_OFFSET_HOURS);

            //TODO -- partial hack to avoid cross referencing - refactor
            ElasticClientSingleton.Configure(_config);

            var searchResponse = ElasticClientSingleton.Instance.Search<ElasticErrorMessage>(x => x
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
                        .GreaterThanOrEquals(startDateTime)
                        .LessThanOrEquals(endDateTime)
                    )
                )))
            );


            if (searchResponse != null && searchResponse.Documents != null)
            {
                return searchResponse.Hits
                    .Select(h =>
                    {
                        h.Source.ElasticId = h.Id;
                        return h.Source;
                    })
                    .ToList();
            }

            return new List<ElasticErrorMessage>();
        }
    }
}
