using System;
using System.Collections.Generic;
using System.Text;

namespace AlertsAdmin.Monitor.Logic
{
    public interface IDataProcessor<in TRecord>
    {
        void Process(TRecord message);
    }
}
