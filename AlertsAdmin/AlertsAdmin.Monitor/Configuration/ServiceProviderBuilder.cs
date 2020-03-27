using AlertsAdmin.Data.Contexts;
using AlertsAdmin.Elastic;
using AlertsAdmin.Elastic.Models;
using AlertsAdmin.Monitor.Collector;
using AlertsAdmin.Monitor.Filters;
using AlertsAdmin.Monitor.Logic;
using AlertsAdmin.Monitor.Notifiers;
using AlertsAdmin.Monitor.Scheduler;
using EFCore.DbContextFactory.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;

namespace AlertsAdmin.Monitor.Configuration
{
    internal class ServiceProviderBuilder
    {
        public static IServiceProvider Build(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<IJobFactory>(provider => new SchedulerJobFactory(provider));

            services.AddTransient<Elastic.ISourceRepository<ElasticErrorMessage>, ElasticDataRepository>();

            services.AddTransient<IDataProcessor<ElasticErrorMessage>, AlertDataProcessor>();

            services.AddSingleton<MessageCollectorJob>();

            services.AddOptions<CustomFiltersOptions>();
            services.Configure<CustomFiltersOptions>(configuration.GetSection("customFilters"));

            // force preloading
            services.AddSingleton<IFilterDefinitions, FilterDefinitions>();

            services.AddDbContextFactory<AlertMonitoringContext>(builder => builder
                .UseSqlServer(configuration.GetConnectionString("AlertMonitoring")));

            services.AddTransient<IAlertMonitoringRepository, AlertMonitoringRepository>();

            // -- multiple subscribers can be added using the same interface
            services.AddTransient<INotificationSubscriber<ElasticErrorMessage>, PlaceHolderEmailSubscriber>();

            services.AddTransient<INotificationPublisher<ElasticErrorMessage>, AlertUpdatePublisher>();

            return services.BuildServiceProvider();
        }
    }
}