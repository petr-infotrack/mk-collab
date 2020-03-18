using System;
using System.Collections.Generic;
using System.Text;
using AlertsAdmin.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace AlertsAdmin.Data.Contexts
{
    public class AlertMonitoringContext : DbContext
    {
        public AlertMonitoringContext() { }
        public AlertMonitoringContext(DbContextOptions options)
            : base(options) { }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlertInstance>()
                .HasOne(ai => ai.Alert)
                .WithMany(a => a.Instances)
                .HasForeignKey(ai => ai.AlertId);

            modelBuilder.Entity<AlertInstance>()
                .HasOne(ai => ai.MessageType)
                .WithMany(m => m.Instances)
                .HasForeignKey(ai => ai.MessageTypeId);

            // -- links MessageType -> Alerts (both ways)
            modelBuilder.Entity<MessageType>()
                .HasOne<Alert>(or => or.Alert)
                .WithOne(lr => lr.MessageType)
                .HasForeignKey<Alert>(k => k.MessageTypeId);
            
            //modelBuilder.Entity<Alert>()
            //    .HasOne<MessageType>(a => a.MessageType)
            //    .WithOne(s => s.Alert)
            ////   // .HasForeignKey(b => b.);
            //    .HasForeignKey<MessageType>(b => b.Id);

            //modelBuilder.Entity<Alert>()
            //    .HasOne<AlertInstance>(a => a.FirstInstance)
            //    .WithOne(x => x.Alert)
            //    .HasForeignKey<AlertInstance>(a => a.AlertId);

            //modelBuilder.Entity<Alert>()
            //    .HasOne(a => a.LastInstance)
            //    .WithMany()
            //    .HasForeignKey(a => a.LastInstanceId);
        }

        public DbSet<AlertInstance> AlertInstances { get; set; }

        public DbSet<Alert> Alerts { get; set; }

        public DbSet<MessageType> MessageTypes { get; set; }

        public DbSet<QueueHistoryRecord> QueueHistory { get; set; }
    }
}
