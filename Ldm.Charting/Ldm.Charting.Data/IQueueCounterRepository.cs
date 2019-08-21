using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldm.Charting.Data
{
    public interface IQueueCounterRepository
    {
        int GetOrderUpdatesCount(string Identifier);
        int GetQueueCount(string Table);
        int GetOrderUpdatesCountWhere(string Where);
        int GetQueueFromQuery(string Query);
        int GetPencilOrderUpdateCount();
        int GetPencilCreateUpdateCount();
    }
}
