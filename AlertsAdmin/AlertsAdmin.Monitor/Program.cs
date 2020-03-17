using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Monitor.Configuration;
using AlertsAdmin.Monitor.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Topshelf;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
    using Microsoft.Extensions.Configuration.Json;


namespace AlertsAdmin.Monitor
{
    internal class Program
    {
        internal static IConfiguration Configuration { get; set; }

        private static void Main(string[] args)
        {
            //Configuration = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .Build();
            


            var serviceProvider = ServiceProviderBuilder.Build(Configuration, new ServiceCollection());

            HostFactory.Run(hostConfig =>
            {
                hostConfig.SetServiceName("AlertMonitorService");
                hostConfig.SetDisplayName("Alert Monitor Service");
                hostConfig.SetDescription("Alert Monitor Service");

                hostConfig.RunAsLocalSystem();

                hostConfig.Service<SchedulingService>(serviceConfig =>
                {
                    var jobFactory = serviceProvider.GetRequiredService<IJobFactory>();

                    serviceConfig.ConstructUsing(() => new SchedulingService(jobFactory));

                    serviceConfig.WhenStarted((service, host) => service.OnStart());

                    serviceConfig.WhenStopped((service, host) => service.OnStop());
                });
            });
        }
    }
}