using Ldm.Alerts.Service.Dependencies;
using Serilog;
using Topshelf;
using Topshelf.Ninject;

using Ldm.Alerts.Service.Interfaces;

namespace Ldm.Alerts.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {


#if DEBUG
            // DEBUG ONLY
            ILogger configuration = new LoggerConfiguration()
               .WriteTo.RollingFile(@"c:\temp\AlertMonitorLog-{Date}.txt")
               .CreateLogger();
#endif

            HostFactory.Run(hostConfig =>
            {
                hostConfig.SetDescription("Alert Monitoring");
                hostConfig.SetDisplayName("Alert Monitoring");
                hostConfig.SetServiceName("Alert Monitoring");

                //  Logger will be injected via Ninject not Topshelf
                //hostConfig.UseSerilog(configuration);

                hostConfig.UseNinject(new ServiceDependencies());


                hostConfig.EnableServiceRecovery(serviceRecovery =>
                {
                    // first failure, 5 minute delay
                    serviceRecovery.RestartService(5);

                    //TODO second failure - create event log entry and shut down the dataService
                    serviceRecovery.TakeNoAction();

                });

                hostConfig.DependsOnEventLog();

                hostConfig.UseAssemblyInfoForServiceInfo();

                hostConfig.StartManually();

                hostConfig.RunAsLocalService();


                hostConfig.Service<IWindowsService>(serviceConfig =>
                {
                    serviceConfig.ConstructUsingNinject();

                    serviceConfig.WhenStarted((s, h) => s.Start(h));
                    serviceConfig.WhenStopped((s, h) => s.Stop(h));

                    serviceConfig.WhenPaused((s, h) => s.Pause(h));
                    serviceConfig.WhenContinued((s, h) => s.Resume(h));
                    serviceConfig.WhenShutdown((s, h) => s.Shutdown(h));
                });
            });
        }

    }

}
