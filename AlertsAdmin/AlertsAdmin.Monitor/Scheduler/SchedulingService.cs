using AlertsAdmin.Monitor.Collector;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace AlertsAdmin.Monitor.Scheduler
{
    public class SchedulingService
    {
        private readonly IJobFactory _jobFactory;
        private readonly IConfiguration _configuration;

        public SchedulingService(IJobFactory jobFactory, IConfiguration configuration)
        {
            _jobFactory = jobFactory;
            _configuration = configuration;
        }

        public bool OnStart()
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.JobFactory = _jobFactory;

            scheduler.Start().Wait();

            var scanInterval = _configuration.GetValue<int>("collector_interval");

            var collectorJob = JobBuilder.Create<MessageCollectorJob>()
                .WithIdentity(JobKey.Create("collector_job"))
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(new TriggerKey("collector_job"))
                .StartNow()
                .WithSimpleSchedule(builder =>
                {
                    builder.WithIntervalInSeconds(scanInterval)
                        .RepeatForever();
                })
                .Build();

            scheduler.ScheduleJob(collectorJob, trigger).Wait();

            return true;
        }

        public bool OnStop()
        {
            return true;
        }
    }
}