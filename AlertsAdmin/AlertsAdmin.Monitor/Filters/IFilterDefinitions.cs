using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace AlertsAdmin.Monitor.Filters
{
    public interface IFilterDefinitions
    {
        List<FilterDefinition> GetFilters(ICollection<string> applicableRange);
        FilterDefinitions LoadFromConfiguration(IOptions<CustomFiltersOptions> configuration);
        int Count();
    }
}