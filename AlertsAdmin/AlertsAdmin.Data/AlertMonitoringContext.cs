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
        public AlertMonitoringContext() { }
        public AlertMonitoringContext(DbContextOptions options)
            : base(options) { }
        
        public DbSet<AlertInstance> AlertInstances { get; set; }

        public DbSet<Alert> Alerts { get; set; }

        public DbSet<MessageType> MessageTypes { get; set; }


    }
}
