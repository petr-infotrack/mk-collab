using System;
using Nest;

namespace AlertsAdmin.Elastic
{
    public class ElasticClientSingleton
    {
        private static ElasticClient instance;
        private static readonly string UserName = ElasticVariablesDebug.Username;
        private static readonly string Password = ElasticVariablesDebug.Password;

        private static readonly string EndPoint = ElasticVariablesDebug.EndPoint;

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
            var url = new Uri(EndPoint);
            var settings = new ConnectionSettings(url)
                .DisableDirectStreaming()
                .BasicAuthentication(UserName, Password);
            var client = new ElasticClient(settings);
            return client;
        }
    }
}