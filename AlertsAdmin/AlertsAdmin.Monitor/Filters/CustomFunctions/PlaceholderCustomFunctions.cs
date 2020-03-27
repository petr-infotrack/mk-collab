using AlertsAdmin.Elastic.Models;

namespace AlertsAdmin.Monitor.Filters.CustomFunctions
{
    public class PlaceholderCustomFunctions
    {
        public bool IsIISError(ElasticErrorMessage msg)
        {
            // -- EXAMPLE ONLY error filter

            return msg.Level.Equals("Error", System.StringComparison.CurrentCultureIgnoreCase)
                && msg.Fields.Environment.Contains("Prod")
                && msg.Message.Contains("IIS ");
        }
    }
}