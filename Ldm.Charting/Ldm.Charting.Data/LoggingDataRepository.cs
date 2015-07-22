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
                List<ErrorOcccurences> allErrorsOverThreshold = db.Query<ErrorOcccurences>("Wallboard", 
                    new { numberOfOccurences, timePeriodMinutes, maxScanPeriod },
                    commandType: CommandType.StoredProcedure
                    ).ToList();
                return allErrorsOverThreshold;
            }
        }

    }
}
