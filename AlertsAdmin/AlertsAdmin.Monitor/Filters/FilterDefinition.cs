using System;
using System.Collections.Generic;

namespace AlertsAdmin.Monitor.Filters
{
    public class FilterDefinition
    {
        public class ConditionDefinition
        {
            public string FilterValue { get; set; }

            public string FilterType { get; set; }

            public Func<object, bool> Function { get; set; }
        }

        public string Name { get; set; }

        public List<string> ApplyTo { get; set; }

        public Type Model { get; set; }

        public List<ConditionDefinition> Conditions { get; set; }
    }
}