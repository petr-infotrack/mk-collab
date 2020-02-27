using Ldm.Alerts.Service.Services.Interfaces;
using Ldm.Charting.Data;

namespace Ldm.Alerts.Service.Services
{
    public interface IAlertSourceService 
        : ISourceDataService<ErrorOcccurences, string>
    {
    }
}