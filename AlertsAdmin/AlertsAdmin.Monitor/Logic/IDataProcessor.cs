namespace AlertsAdmin.Monitor.Logic
{
    public interface IDataProcessor<in TRecord>
    {
        void Process(TRecord message);
    }
}