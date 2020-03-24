using AlertsAdmin.Elastic.Models;
using System;

namespace AlertsAdmin.Monitor.Logic
{
    public static class EqualityExtensions
    {
        public static bool IsEqual(this ElasticErrorMessage baseMessage, string compared)
        {
            return baseMessage.MessageTemplate.Equals(compared, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}