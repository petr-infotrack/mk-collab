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

            modelBuilder.Entity<Alert>()
                .HasOne(a => a.MessageType)
                .WithMany()
                .HasForeignKey(a => a.MessageTypeId);

            modelBuilder.Entity<Alert>()
                .HasOne(a => a.FirstInstance)
                .WithMany()
                .HasForeignKey(a => a.FirstInstanceId);

            modelBuilder.Entity<Alert>()
                .HasOne(a => a.LastInstance)
                .WithMany()
                .HasForeignKey(a => a.LastInstanceId);
        }

        public DbSet<AlertInstance> AlertInstances { get; set; }

        public DbSet<Alert> Alerts { get; set; }

        public DbSet<MessageType> MessageTypes { get; set; }

        public DbSet<QueueHistoryRecord> QueueHistory { get; set; }
    }
}
