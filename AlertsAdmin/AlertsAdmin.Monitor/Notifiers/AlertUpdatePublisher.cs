using AlertsAdmin.Elastic.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace AlertsAdmin.Monitor.Notifiers
{
    public class AlertUpdatePublisher : NotificationPublisherBase<ElasticErrorMessage>, INotificationPublisher<ElasticErrorMessage>
    {
        private readonly IConfiguration _configuration;

        public AlertUpdatePublisher(IConfiguration configuration, IEnumerable<INotificationSubscriber<ElasticErrorMessage>> subscribers)
        {
            _configuration = configuration;

            if (subscribers != null)
            {
                foreach (var s in subscribers)
                {
                    Register(s);
                }
            }
        }

        public override void Publish(ElasticErrorMessage data)
        {
            foreach (var subscriber in this.subscribers)
            {
                if (subscriber.ShouldBeNotified(data))
                {
                    subscriber.Notify(data);
                }
            }
        }
    }
}