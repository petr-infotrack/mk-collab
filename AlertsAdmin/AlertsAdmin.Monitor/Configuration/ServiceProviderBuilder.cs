using System;
using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Monitor.Collector;
using AlertsAdmin.Monitor.Scheduler;
using EFCore.DbContextFactory.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;

namespace AlertsAdmin.Monitor.Configuration
{
    internal class ServiceProviderBuilder
    {

        public static IServiceProvider Build(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<IJobFactory>(provider => new SchedulerJobFactory(provider));

            //services.AddSchedulerJobFactory();

            services.AddSingleton<MessageCollectorJob>();


            services.AddDbContextFactory<AlertMonitoringContext>(builder => builder
                .UseSqlServer(configuration.GetConnectionString("AlertMonitoring")));


            return services.BuildServiceProvider();
           
        }
    }


    internal static class ServiceCollectionExtensions {

        public static void AddSchedulerJobFactory(this IServiceCollection service)
        {
            service.AddSingleton<IJobFactory>(provider => new SchedulerJobFactory(provider));
        }
    }
}
