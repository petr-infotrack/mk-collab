using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace AlertsAdmin.Data
{
    public class AlertMonitoringContext : DbContext
    {
        private const string DB_CONNECTION_NAME = "AlertMonitoring";

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            
            builder.UseSqlServer(configuration.GetConnectionString(DB_CONNECTION_NAME));
            
        }


        public DbSet<AlertInstance> AlertInstances { get; set; }

        public DbSet<Alert> Alerts { get; set; }

        public DbSet<MessageType> MessageTypes { get; set; }


    }
}
