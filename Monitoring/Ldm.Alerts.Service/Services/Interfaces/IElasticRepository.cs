using Ldm.Charting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldm.Alerts.Service.Services.Interfaces
{
    public interface IElasticDataRepository
    {

        List<ErrorOcccurences> GetErrorsByContentFilter(string filter, int maxScanPeriod);
        //List<ErrorOcccurences> GetErrorsByScanRange(int numberOfOccurences, int timePeriodMinutes, int maxScanPeriod);
    }

}
