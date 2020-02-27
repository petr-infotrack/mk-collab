using Ldm.Alerts.Service.Services.Interfaces;
using Ldm.Charting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldm.Alerts.Service.Services
{
    public class AlertSourceService : IAlertSourceService //,  ISourceDataService <ErrorOcccurences, string> 
    {
        IElasticDataRepository _elastic;


        public AlertSourceService() { }

        public void Open()
        {

        }

        public void Close()
        {
            
        }

        public ErrorOcccurences Get(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ErrorOcccurences> GetAll(object filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
