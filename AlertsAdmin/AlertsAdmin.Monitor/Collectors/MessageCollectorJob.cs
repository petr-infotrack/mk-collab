using AlertsAdmin.Elastic;
using AlertsAdmin.Elastic.Models;
using AlertsAdmin.Monitor.Logic;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Threading.Tasks;

namespace AlertsAdmin.Monitor.Collector
{
    public class MessageCollectorJob : IJob
    {
        private readonly IConfiguration _configuration;
        private readonly ISourceRepository<ElasticErrorMessage> _source;
        private readonly IDataProcessor<ElasticErrorMessage> _processor;

        public MessageCollectorJob(IConfiguration configuration, ISourceRepository<ElasticErrorMessage> source, IDataProcessor<ElasticErrorMessage> processor)
        {
            _configuration = configuration;
            _source = source;
            _processor = processor;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // TODO NTH -- replace with IOptions
            var elasticConfig = GetElasticVariables(_configuration);

            //TODO - partial hack to avoid cross referencing - refactor
            ((ElasticDataRepository)_source).Configure(elasticConfig);

            var sourceData = _source.GetErrorMessages(DateTime.Now, elasticConfig.scanInterval, 5);

            foreach (var r in sourceData)
            {
                _processor.Process(r);
            }
        }

        private static (string endpoint, string user, string pwd, int scanInterval) GetElasticVariables(IConfiguration configuration)
        {
            return (
                configuration.GetValue<string>("elastic:endpoint"),
                configuration.GetValue<string>("elastic:username"),
                configuration.GetValue<string>("elastic:password"),
                configuration.GetValue<int>("elastic:scan_interval_minutes"));
        }
    }
}