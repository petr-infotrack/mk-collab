namespace AlertsAdmin.Monitor.Notifiers
{
    public interface INotifier<T>
    {
        void Notify(T data);
    }
}