using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlertsAdmin.Data.Migrations
{
    public partial class Init_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    StatusMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Template = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Notification = table.Column<int>(nullable: false),
                    DefaultStatus = table.Column<int>(nullable: false),
                    ExpiryTime = table.Column<TimeSpan>(nullable: false),
                    ExpiryCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlertInstances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElasticId = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    MessageTypeId = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    JsonData = table.Column<string>(nullable: true),
                    AlertId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertInstances_Alerts_AlertId",
                        column: x => x.AlertId,
                        principalTable: "Alerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlertInstances_MessageTypes_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalTable: "MessageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertInstances_AlertId",
                table: "AlertInstances",
                column: "AlertId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertInstances_MessageTypeId",
                table: "AlertInstances",
                column: "MessageTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertInstances");

            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "MessageTypes");
        }
    }
}
