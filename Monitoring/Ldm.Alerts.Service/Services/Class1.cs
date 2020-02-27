using Ldm.Charting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldm.Alerts.Service.Services
{
    public class AlertProcessor : IAlertProcessor<ErrorOcccurences>
    {
        private ITargetDataService _target;

        public AlertProcessor(ITargetDataService target)
        {
           
        }



        public bool Process(ErrorOcccurences record)
        {
            
        }

        public bool Validate(ErrorOcccurences record)
        {
            
        }
    }

    public interface ITargetDataService<TRecord, TKey>
    {
        bool Exists(TKey key);
        TRecord Read(TKey key);
        void Update(TRecord record);
        void Add(TRecord record);

    }
}
