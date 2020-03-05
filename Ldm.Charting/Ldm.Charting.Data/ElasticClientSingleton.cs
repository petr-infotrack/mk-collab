using Nest;
using System;

namespace Ldm.Charting.Data
{
    public class ElasticClientSingleton
    {
        private static ElasticClient instance;
        private static readonly string UserName = System.Configuration.ConfigurationManager.AppSettings["elasticLogsUserName"];
        private static readonly string Password = System.Configuration.ConfigurationManager.AppSettings["elasticLogsPassword"];


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
                .DisableDirectStreaming()
                .BasicAuthentication(UserName, Password);
            var client = new ElasticClient(settings);
            return client;
        }
    }
}
