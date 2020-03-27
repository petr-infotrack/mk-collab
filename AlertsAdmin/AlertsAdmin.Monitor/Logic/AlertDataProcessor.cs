using AlertsAdmin.Elastic.Models;
using AlertsAdmin.Monitor.Filters;
using AlertsAdmin.Monitor.Notifiers;
using Microsoft.Extensions.Configuration;

namespace AlertsAdmin.Monitor.Logic
{
    public class AlertDataProcessor : IDataProcessor<ElasticErrorMessage>
    {
        private const string Map_FilterName = "map-message-type";


        private readonly IConfiguration _configuration;
        private readonly IAlertMonitoringRepository _db;
        private readonly INotificationPublisher<ElasticErrorMessage> _notifier;
        private readonly IFilterDefinitions _filters;

        public AlertDataProcessor(IConfiguration configuration, IAlertMonitoringRepository db, INotificationPublisher<ElasticErrorMessage> notifier, IFilterDefinitions filters)
        {
            _configuration = configuration;
            _db = db;
            _notifier = notifier;
            _filters = filters;
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
            //TODO  PROVISIONAL VERSION - REFACTOR

            if (_configuration.GetValue<bool>("customFilters::active") && _filters.Count() > 0)
            {
                var applicableFilters = _filters.GetFilters(new string[] { Map_FilterName });

                var result = true;

                foreach(var f in applicableFilters)
                {
                    result = result && f.Conditions.TrueForAll(c => c.Function(message));
                }

                return result;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(message.MessageTemplate))
                {
                    return false;
                }

                // TODO - refactor or implement proper regex  - if no custom filter is used 
                if (message.MessageTemplate.IndexOf('{') >= 0 && message.MessageTemplate.IndexOf('}') >= 0)
                {
                    return true;
                }
            }

            return false;
        }

    }
}