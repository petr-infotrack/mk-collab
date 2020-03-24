namespace AlertsAdmin.Monitor.Notifiers
{
    public interface INotificationSubscriber<T> : INotifier<T>
    {
        void Notify(T data);

        bool ShouldBeNotified(T data);
    }
}