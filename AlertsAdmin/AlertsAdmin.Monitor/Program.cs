using AlertsAdmin.Monitor.Configuration;
using AlertsAdmin.Monitor.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System.IO;
using Topshelf;

namespace AlertsAdmin.Monitor
{
    internal class Program
    {
        internal static IConfiguration Configuration { get; set; }

        public static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }

        private static void Main(string[] args)
        {
            Configuration = BuildConfiguration();

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

                    serviceConfig.ConstructUsing(() => new SchedulingService(jobFactory, Configuration));

                    serviceConfig.WhenStarted((service, host) => service.OnStart());

                    serviceConfig.WhenStopped((service, host) => service.OnStop());
                });
            });
        }
    }
}