namespace AlertsAdmin.Monitor.Notifiers
{
    public interface INotificationPublisher<T>
    {
        void Register(INotificationSubscriber<T> subscriber);

        void Unregister(INotificationSubscriber<T> subscriber);

        void Publish(T data);
    }
}