namespace Ldm.Alerts.Service.Services
{
    public interface IAlertProcessor<TRecord>
    {
        bool Validate(TRecord record);

        bool Process(TRecord record);

    }
}
