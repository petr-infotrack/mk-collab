using System;
using Nest;

namespace AlertsAdmin.Elastic
{
    [ElasticsearchType(Name = "logevent")]
    public class ErrorOcccurences
    {
        [Text(Name = "messageTemplate")]
        public string ErrorMessage { get; set; }

        [Text(Name = "error_counts")]
        public int Occurrences { get; set; }

        public int FirstOccurrence { get; set; }

        [Text(Name = "@timestamp")]
        public DateTime Timestamp { get; set; }

        [Text(Name = "level")]
        public string Level { get; set; }

        [Text(Name = "fields")]
        public Fields Fields { get; set; }

        public ErrorOcccurences() { }

        public ErrorOcccurences(string errorMessage, int occurrences, int firstOccurrence)
        {
            ErrorMessage = errorMessage;
            Occurrences = occurrences;
            FirstOccurrence = firstOccurrence;
        }
    }
}