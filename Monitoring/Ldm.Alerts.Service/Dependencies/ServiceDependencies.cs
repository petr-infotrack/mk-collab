using Ldm.Alerts.Service.Interfaces;
using Ldm.Alerts.Service.Services;
using Ninject.Modules;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ldm.Alerts.Service.Dependencies
{
    public class ServiceDependencies : NinjectModule
    {

        private static ILogger CreateSerilogLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.RollingFile(@"C:\temp\oilog-{Date}.txt")
                .CreateLogger();
        }


        public override void Load()
        {

            Bind<ILogger>().ToMethod(m => CreateSerilogLogger());

            Bind<IWindowsService>().To<AlertMonitorService>().InSingletonScope();

            Bind<IScheduledJob>().To<CollectorNotifierJob>();

        }
    }
}
