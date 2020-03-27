using System.Collections.Generic;

namespace AlertsAdmin.Monitor.Filters
{
    public class CustomFiltersOptions
    {
        public class Filter
        {
            public string Name { get; set; }
            public string Model { get; set; }
            public List<string> ApplyTo { get; set; }
            public List<MatchQuery> Match { get; set; }
        }

        public class MatchQuery
        {
            public string Field { get; set; }
            public string QueryType { get; set; }

            public string QueryValue { get; set; }
        }

        public List<Filter> filters { get; set; }

        public bool active { get; set; }
    }
}