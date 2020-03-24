using System.Collections.Generic;

namespace AlertsAdmin.Monitor.Notifiers
{
    public abstract class NotificationPublisherBase<T> : INotificationPublisher<T>
    {
        protected IList<INotificationSubscriber<T>> subscribers = new List<INotificationSubscriber<T>>();

        public void Register(INotificationSubscriber<T> subscriber)
        {
            if (subscriber == null)
            {
                return;
            }

            subscribers.Add(subscriber);
        }

        public void Unregister(INotificationSubscriber<T> subscriber)
        {
            var idx = subscribers.IndexOf(subscriber);
            if (idx >= 0)
            {
                subscribers.RemoveAt(idx);
            }
        }

        public abstract void Publish(T data);
    }
}