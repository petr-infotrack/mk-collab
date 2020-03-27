using AlertsAdmin.Monitor.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlertsAdmin.Monitor.Filters
{
    public class FilterDefinitions : IFilterDefinitions
    {
        public FilterDefinitions(IOptions<CustomFiltersOptions> filterOptions)
        {
            this.LoadFromConfiguration(filterOptions);
        }
       

        private List<FilterDefinition> _filters = new List<FilterDefinition>();

        public FilterDefinitions LoadFromConfiguration(IOptions<CustomFiltersOptions> filterOptions)
        {

            if (filterOptions.Value.active)
            {
                try
                {
                    _filters = new FilterDefinitionBuilder().Build(filterOptions.Value);
                }
                catch (Exception ex)
                {
                    // Add handling - eg. log
                }
            }

            return this;
        }

        public int Count()
        {
            return _filters?.Count() ?? 0;
        }

        public List<FilterDefinition> GetFilters(ICollection<string> applicableRange)
        {
            return _filters
                .Where(w => w.ApplyTo.Any(x => applicableRange.Contains(x)))
                .ToList();
        }

    }
}