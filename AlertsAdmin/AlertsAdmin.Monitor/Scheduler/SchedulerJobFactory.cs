using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace AlertsAdmin.Monitor.Scheduler
{
    public class SchedulerJobFactory : IJobFactory
    {
        private readonly IServiceProvider _provider;

        public SchedulerJobFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _provider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            
            if (job == null)
            {
                throw new NotSupportedException($"{bundle.JobDetail.JobType.Name} is not supported!");
            }

            return job;
        }


        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;

            disposable?.Dispose();
        }
    }
}
