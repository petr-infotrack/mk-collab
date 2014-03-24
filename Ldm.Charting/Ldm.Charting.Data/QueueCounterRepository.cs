using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Ldm.Charting.Data
{
    public class QueueCounterRepository : IQueueCounterRepository
    {
        private readonly IDbConnection _db = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["ldmCore"].ConnectionString);

        public int GetOrderUpdatesCount(string Identifier)
        {
            return _db.Query<int>("SELECT COUNT(*) QueueLength FROM ldmcore.dbo.orderupdates WITH (NOLOCK) where Identifier like @Identifier", new { Identifier = Identifier }).Single();
        }

        public int GetQueueCount(string Table)
        {
            return _db.Query<int>(String.Format("SELECT COUNT(*) QueueLength FROM {0} WITH (NOLOCK)", Table)).Single();
        }

        public int GetOrderUpdatesCountWhere(string Where)
        {
            var query =
                String.Format("SELECT COUNT(*) QueueLength FROM ldmcore.dbo.orderupdates WITH (NOLOCK) WHERE {0}", Where);
            return _db.Query<int>(query).Single();
        }
    }
}
