﻿// <auto-generated />
using System;
using AlertsAdmin.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlertsAdmin.Data.Migrations
{
    [DbContext(typeof(AlertMonitoringContext))]
    partial class AlertMonitoringContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AlertsAdmin.Domain.Models.Alert", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AckCount")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("AckTime")
                        .HasColumnType("time");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("FirstInstanceId")
                        .HasColumnType("int");

                    b.Property<int>("InstanceCount")
                        .HasColumnType("int");

                    b.Property<int>("LastInstanceId")
                        .HasColumnType("int");

                    b.Property<int>("MessageTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MessageTypeId")
                        .IsUnique();

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("AlertsAdmin.Domain.Models.AlertInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlertId")
                        .HasColumnType("int");

                    b.Property<string>("ElasticId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JsonData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MessageTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AlertId");

                    b.HasIndex("MessageTypeId");

                    b.ToTable("AlertInstances");
                });

            modelBuilder.Entity("AlertsAdmin.Domain.Models.MessageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DefaultStatus")
                        .HasColumnType("int");

                    b.Property<int>("ExpiryCount")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("ExpiryTime")
                        .HasColumnType("time");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("Notification")
                        .HasColumnType("int");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Template")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThresholdCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MessageTypes");
                });

            modelBuilder.Entity("AlertsAdmin.Domain.Models.QueueHistoryRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("QueueName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("QueueHistory");
                });

            modelBuilder.Entity("AlertsAdmin.Domain.Models.Alert", b =>
                {
                    b.HasOne("AlertsAdmin.Domain.Models.MessageType", "MessageType")
                        .WithOne("Alert")
                        .HasForeignKey("AlertsAdmin.Domain.Models.Alert", "MessageTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AlertsAdmin.Domain.Models.AlertInstance", b =>
                {
                    b.HasOne("AlertsAdmin.Domain.Models.Alert", "Alert")
                        .WithMany("Instances")
                        .HasForeignKey("AlertId");

                    b.HasOne("AlertsAdmin.Domain.Models.MessageType", "MessageType")
                        .WithMany("Instances")
                        .HasForeignKey("MessageTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
