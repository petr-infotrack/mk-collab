using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Interfaces;
using AlertsAdmin.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Enums;
using AlertsAdmin.Domain.Extensions;

namespace AlertsAdmin.Data.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly Func<LdmCoreContext> _factory;
        private LdmCoreContext _db => _factory.Invoke();

        private const string TABLE_COUNT_TEMPLATE = "SELECT 1 QueueLength FROM {0} WITH (NOLOCK)";
        private const string ORDERUPDATES_COUNT_TEMPLATE = @"SELECT 1 FROM LdmCore.dbo.OrderUpdates WITH (NOLOCK) WHERE Identifier{0}LIKE '%WcfReceiver_OfficeExpressIntegrationService%'";

        public QueueRepository(Func<LdmCoreContext> factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<KeyValuePair<string,int>>> GetQueueData()
        {
            var data = new List<KeyValuePair<string, int>>();
            int length;
            foreach(QueueTable key in Enum.GetValues(typeof(QueueTable)))
            {
                length = await GetQueueCountAsync(key);
                
                data.Add(new KeyValuePair<string, int>(key.ToString(), length));
            }
            length = await GetOrderUpdatesAsync();
            data.Add(new KeyValuePair<string, int>("OrderUpdates", length));
            length = await GetOrderUpdatesAsync();
            data.Add(new KeyValuePair<string, int>("OfficeExpressIntegration", length));
            return data;
        }

        #region PrivateMethods

        private async Task<int> GetQueueCountAsync(QueueTable table)
        {
            using (var context = _db)
            {
                var sql = String.Format(TABLE_COUNT_TEMPLATE, table.ToString());
                var retVal = await context.Database.ExecuteSqlRawAsync(sql);
                // ExecuteRawSQL has a querk where it returns -1 for 0 rows;                   
                if (retVal == -1)
                    retVal = 0;
                return retVal;
            }
        }

        private async Task<int> GetOrderUpdatesAsync(bool withLeap = false)
        {
            using (var context = _db)
            {
                string sql;
                if (withLeap)
                    sql = String.Format(ORDERUPDATES_COUNT_TEMPLATE, "");
                else
                    sql = String.Format(ORDERUPDATES_COUNT_TEMPLATE, " NOT ");

                var retVal = await context.Database.ExecuteSqlRawAsync(sql);
                // ExecuteRawSQL has a querk where it returns -1 for 0 rows;                   
                if (retVal == -1)
                    retVal = 0;
                return retVal;
            }
        }



        #endregion
    }
}
