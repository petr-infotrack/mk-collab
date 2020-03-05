using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Ldm.Charting.Data
{
    public class LdmAlertDataRepository : ILoggingDataRepository
    {
        public List<ErrorOcccurences> GetAllErrorsOverThreshold(int numberOfOccurences, int timePeriodMinutes, int maxScanPeriod)
        {
            var errorsOverThreshold = new List<ErrorOcccurences>();
            try
            {                
                using (var db = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["ldmLogging"].ConnectionString))
                {
                    errorsOverThreshold.AddRange(db.Query<ErrorOcccurences>(
                        "WallBoardLDMAlert",
                        new
                        {
                            numberOfOccurences,
                            timePeriodMinutes
                        },
                        commandType: CommandType.StoredProcedure)
                    );
                }
            }
            catch (Exception e)
            {
                errorsOverThreshold.Add(new ErrorOcccurences($"Error reading LDM alert errors {e.Message}", 1, 0));
            }

            return errorsOverThreshold;
        }
    }
}
