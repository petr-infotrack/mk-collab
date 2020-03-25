using System;
using Nest;

namespace AlertsAdmin.Elastic
{
    public class ElasticClientSingleton
    {
        private static ElasticClient instance;
        private static string UserName = ElasticVariablesPreset.Username;
        private static string Password = ElasticVariablesPreset.Password;

        private static string EndPoint = ElasticVariablesPreset.EndPoint;

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

        public static void Configure((string endpoint, string user, string pwd, int scanInterval) config)
        {
            UserName = config.user;
            Password = config.pwd;
            EndPoint = config.endpoint;
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