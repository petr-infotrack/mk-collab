using Nest;
using System;

namespace Ldm.Charting.Data
{
    public class ElasticClientSingleton
    {
        private static ElasticClient instance;

        private ElasticClientSingleton() { }

        public static ElasticClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GetElasticClient();
                }
                return instance;
            }
        }

        private static ElasticClient GetElasticClient()
        {
            var url = new Uri(System.Configuration.ConfigurationManager.AppSettings["elasticLogsEndpoint"]);
            var settings = new ConnectionSettings(url)
                .DisableDirectStreaming();
            var client = new ElasticClient(settings);
            return client;
        }
    }
}
