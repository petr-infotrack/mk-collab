using Nest;
using System;

namespace Ldm.Charting.Data
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

    public class Fields
    {
        public string MachineName { get; set; }

        public string Application { get; set; }

        [Text(Name = "orderid")]
        public string OrderIdStr { get; set; }

        [Text(Name = "OrderId")]
        public int OrderIdInt { get; set; }

        public string Environment { get; set; }

        public string Region { get; set; }
    }

}
