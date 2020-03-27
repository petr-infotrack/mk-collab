using AlertsAdmin.Elastic.Models;
using AlertsAdmin.Monitor.Filters;
using AlertsAdmin.Monitor.Filters.CustomFunctions;
using AlertsAdmin.Monitor.Notifiers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AlertsAdmin.Monitor.Logic
{
    public class AlertDataProcessor : IDataProcessor<ElasticErrorMessage>
    {
        private const string FilterApplyName = "map-message-type";

        private readonly IAlertMonitoringRepository _db;
        private readonly INotificationPublisher<ElasticErrorMessage> _notifier;
        private readonly IFilterDefinitions _filters;
        private readonly IOptions<CustomFiltersOptions> _filterOptions;

        public AlertDataProcessor(IOptions<CustomFiltersOptions> options,
                                  IFilterDefinitions filters,
                                  IAlertMonitoringRepository db,
                                  INotificationPublisher<ElasticErrorMessage> notifier)
        {
            _db = db;
            _notifier = notifier;
            _filters = filters;
            _filterOptions = options;
        }

        public void Process(ElasticErrorMessage message)
        {
            if (IsMonitored(message))
            {
                // TODO -- refactor after debugging to async
                _db.AddMessageAsync(message).Wait();

                _notifier?.Publish(message);
            }
        }

        private bool IsMonitored(ElasticErrorMessage message)
        {
            if (_filterOptions.Value.active && _filters.Count() > 0)
            {
                var applicableFilters = _filters.GetFilters(new string[] { FilterApplyName });

                return applicableFilters.TrueForAll(f =>
                    f.Conditions.TrueForAll(c => c.Function(message))
                );
            }
            else
            {
                return (new DefaultFilterFunctions().IsMessageMonitored(message));
            }
        }
    }
}