using Dapper;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace Ldm.Charting.Data
{
    public interface ILoggingDataRepository
    {
        List<ErrorOcccurences> GetAllErrorsOverThreshold(int numberOfOccurences, int timePeriodMinutes, int maxScanPeriod);
    }

    public class LoggingDataRepository : ILoggingDataRepository
    {
        private static readonly HashSet<string> EligibleEnvironements = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            "Production",
            "Live",
            "Production-AU",
            "Prod"
        };

        public List<ErrorOcccurences> GetAllErrorsOverThreshold(int numberOfOccurences, int timePeriodMinutes, int maxScanPeriod)
        {
            var errorsOverThreshold = new List<ErrorOcccurences>();

            using (var db = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["ldmLogging"].ConnectionString))
            {
                errorsOverThreshold.AddRange(db.Query<ErrorOcccurences>("Wallboard",
                    new { numberOfOccurences, timePeriodMinutes, maxScanPeriod },
                    commandType: CommandType.StoredProcedure
                ));
            }

            try
            {
                var elasticSearchErrors = GetElasticSearchErrors(numberOfOccurences, timePeriodMinutes, maxScanPeriod);
                errorsOverThreshold.AddRange(elasticSearchErrors);
            }
            catch (Exception e)
            {
                errorsOverThreshold.Add(new ErrorOcccurences($"Error reading from Kibana {e.Message}", 1, 0));
            }
            
            return errorsOverThreshold;
        }

        private List<ErrorOcccurences> GetElasticSearchErrors(int numberOfOccurences, int timePeriodMinutes, int maxScanPeriod)
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
                response = searchResponse.Documents
                    .Where(d => d.Fields.Environment == null
                    || EligibleEnvironements.Contains(d.Fields.Environment))
                    .GroupBy(d => d.ErrorMessage)
                    .Where(g => g.Any(i => i.Timestamp > DateTime.Now.AddMinutes((timePeriodMinutes * -1))) // where there has been at least one occurrence in the past [timePeriodMinutes] minutes
                        && g.Count() >= numberOfOccurences // and where there have been at least [numberOfOccurences] occurrences
                    )
                    .Select(g => new ErrorOcccurences
                    {
                        ErrorMessage = GetErrorMessage(g),
                        Occurrences = g.Count(),
                        FirstOccurrence = g.Select(f => f.Timestamp).Min().Minute,
                        Fields = g.First().Fields
                    }).ToList();   
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
