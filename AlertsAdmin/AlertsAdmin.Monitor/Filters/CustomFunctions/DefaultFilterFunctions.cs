using AlertsAdmin.Elastic.Models;
using System.Text.RegularExpressions;

namespace AlertsAdmin.Monitor.Filters.CustomFunctions
{
    public class DefaultFilterFunctions
    {
        public bool IsMessageMonitored(ElasticErrorMessage message)
        {
            // default filter function - template (refactor)
            var regex = new Regex(@"\{.*?\}");

            return regex.Match(message.MessageTemplate).Success;
        }
    }
}