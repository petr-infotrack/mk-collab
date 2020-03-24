using AlertsAdmin.Elastic.Models;
using AlertsAdmin.Monitor.Notifiers;
using Microsoft.Extensions.Configuration;

namespace AlertsAdmin.Monitor.Logic
{
    public class AlertDataProcessor : IDataProcessor<ElasticErrorMessage>
    {
        private readonly IConfiguration _configuration;
        private readonly IAlertMonitoringRepository _db;
        private readonly INotificationPublisher<ElasticErrorMessage> _notifier;

        public AlertDataProcessor(IConfiguration configuration, IAlertMonitoringRepository db, INotificationPublisher<ElasticErrorMessage> notifier)
        {
            _configuration = configuration;
            _db = db;
            _notifier = notifier;
        }

        public void Process(ElasticErrorMessage message)
        {
            // TODO -- refactor after debugging to async
            _db.AddMessageAsync(message).Wait();

            _notifier?.Publish(message);
        }
    }
}