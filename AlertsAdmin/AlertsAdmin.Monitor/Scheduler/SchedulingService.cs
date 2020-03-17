using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AlertsAdmin.Monitor.Collector;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace AlertsAdmin.Monitor.Scheduler
{
    public class SchedulingService
    {
        private readonly IJobFactory _jobFactory;

        public SchedulingService(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public bool OnStart()
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            scheduler.JobFactory = _jobFactory;

            scheduler.Start().Wait();

            var collectorJob = JobBuilder.Create<MessageCollectorJob>()
                .WithIdentity(JobKey.Create("collector_job"))
                .Build();

            
            var trigger = TriggerBuilder.Create()
                .WithIdentity(new TriggerKey("collector_job"))
                .StartNow()
                .WithSimpleSchedule(builder =>
                {
                    builder.WithIntervalInSeconds(30)
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
