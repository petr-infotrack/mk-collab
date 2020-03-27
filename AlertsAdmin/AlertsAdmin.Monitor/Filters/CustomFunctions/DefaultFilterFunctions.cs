using AlertsAdmin.Elastic.Models;

namespace AlertsAdmin.Monitor.Filters.CustomFunctions
{
    public class DefaultFilterFunctions
    {
        public bool IsMessageMonitored(ElasticErrorMessage msg)
        {
            // default filter function - no filter
            return true;
        }
    }
}