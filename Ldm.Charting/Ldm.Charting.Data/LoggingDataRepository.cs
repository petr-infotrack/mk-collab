using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Ldm.Charting.Data
{
    public interface ILoggingDataRepository
    {
        List<ErrorOcccurences> GetAllErrorsOverThreshold(int numberOfOccurences, int timePeriodMinutes, int maxScanPeriod);
    }

    public class ErrorOcccurences
    {
        public string ErrorMessage { get; set; }
        public int Occurrences { get; set; }
        public int FirstOccurrence { get; set; }

        public ErrorOcccurences(string errorMessage, int occurrences, int firstOccurrence)
        {
            ErrorMessage = errorMessage;
            Occurrences = occurrences;
            FirstOccurrence = firstOccurrence;
        }
    }


    public class LoggingDataRepository : ILoggingDataRepository
    {
        public List<ErrorOcccurences> GetAllErrorsOverThreshold(int numberOfOccurences, int timePeriodMinutes, int maxScanPeriod)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["ldmLogging"].ConnectionString))
            {
                List<ErrorOcccurences> allErrorsOverThreshold = db.Query<ErrorOcccurences>(string.Format(
                    "select distinct e.errormessage as errorMessage, " +
                    "count(o.OccurenceId) as occurrences, " +
                    "DATEDIFF(mi, min(firstOccurence.thrownat), getdate()) as firstOccurrence " +
                    "from errors e with (nolock)" +
                    "inner join occurences o with (nolock) on e.errorid = o.errors_errorid and o.ThrownAt > DateADD(mi, -{0}, Current_TimeStamp) " +
                    "inner join Occurences firstOccurence with (nolock) on e.errorid = firstOccurence.errors_errorid and firstOccurence.ThrownAt > DateADD(mi, -{2}, Current_TimeStamp) " +
                    "and e.SourceComputer like 'mt%' " +
                    "group by e.errorid,e.errormessage, o.OccurenceId " +
                    "having count(o.OccurenceId) >{1} " +
                    "and max(o.thrownat) > DateADD(SECOND, -30, Current_TimeStamp) " +
                    "order by  firstOccurrence desc"
                    , timePeriodMinutes, numberOfOccurences, maxScanPeriod)).ToList();
                return allErrorsOverThreshold;
            }
        }

    }
}
