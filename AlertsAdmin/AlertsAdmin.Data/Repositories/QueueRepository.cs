using System;
using System.Collections.Generic;
using AlertsAdmin.Domain.Interfaces;
using System.Threading.Tasks;
using AlertsAdmin.Domain.Extensions;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;
using AlertsAdmin.Domain.Attributes;

namespace AlertsAdmin.Data.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly IDbConnection _db;

        private const string TABLE_COUNT_TEMPLATE = "SELECT COUNT(*) QueueLength FROM {0} WITH (NOLOCK)";

        public QueueRepository(IConfiguration config) 
            => _db = new System.Data.SqlClient.SqlConnection(config.GetConnectionString("LdmCore"));

        public async Task<IEnumerable<KeyValuePair<string,int>>> GetQueueDataAsync<T>()
        
        {
            if (!EnumExtensions.IsQueueTable<T>())
                throw new ArgumentException($"{typeof(T).ToString()} does not have the QueueTableAttribute");

            var data = new List<KeyValuePair<string, int>>();

            int length;
            foreach (Enum key in Enum.GetValues(typeof(T)))
            {
                length = await GetQueueCountAsync(key);

                string queueName;
                if (!key.TryGetQueueName(out queueName))
                    queueName = key.ToString();

                data.Add(new KeyValuePair<string, int>(queueName, length));
            }
            return data;
        }

        #region PrivateMethods

        /// <summary>
        /// <para>Returns the length of an <see cref="Enum"/> that uses <see cref="QueueTableAttribute"/></para>
        /// <para>Can use the <see cref="QueryAttribute"/> to specify a query to generate the data</para>
        /// <para>Can use the <see cref="QueueResourceAttribute"/> to specify a queue name that does not comply to an enum name</para>
        /// </summary>
        /// <param name="queue"></param>
        /// <returns>The number of items in the queue</returns>
        private async Task<int> GetQueueCountAsync(Enum queue)
        {
            int length;
            if (queue.TryGetQuery(out var query))
                length = await GetQueueCountByQueryAsync(query);
            else
            {
                if (queue.TryGetResource(out var resource))
                    length = await GetQueueCountByNameAsync(resource);
                else
                    length = await GetQueueCountByNameAsync(queue.ToString());
            }
            return length;
        }

        private async Task<int> GetQueueCountByQueryAsync(string query)
        {
            var result = await _db.QueryAsync<int>(query);
            return result.Single();
        }

        private async Task<int> GetQueueCountByNameAsync(string queueTable)
        {
            var query = String.Format(TABLE_COUNT_TEMPLATE, queueTable);
            var result = await _db.QueryAsync<int>(query);
            return result.Single();            
        }

        #endregion
    }
}
