using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlertsAdmin.Data.Migrations
{
    public partial class Added_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertInstances_MessageTypes_MessageTypeId",
                table: "AlertInstances");

            migrationBuilder.AddColumn<int>(
                name: "ThresholdCount",
                table: "MessageTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AckCount",
                table: "Alerts",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AckTime",
                table: "Alerts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Alerts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FirstInstanceId",
                table: "Alerts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstanceCount",
                table: "Alerts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastInstanceId",
                table: "Alerts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MessageTypeId",
                table: "Alerts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Alerts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "Alerts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "MessageTypeId",
                table: "AlertInstances",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "QueueHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueueName = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_MessageTypeId",
                table: "Alerts",
                column: "MessageTypeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertInstances_MessageTypes_MessageTypeId",
                table: "AlertInstances",
                column: "MessageTypeId",
                principalTable: "MessageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_MessageTypes_MessageTypeId",
                table: "Alerts",
                column: "MessageTypeId",
                principalTable: "MessageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertInstances_MessageTypes_MessageTypeId",
                table: "AlertInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_MessageTypes_MessageTypeId",
                table: "Alerts");

            migrationBuilder.DropTable(
                name: "QueueHistory");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_MessageTypeId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "ThresholdCount",
                table: "MessageTypes");

            migrationBuilder.DropColumn(
                name: "AckCount",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "AckTime",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "FirstInstanceId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "InstanceCount",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "LastInstanceId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "MessageTypeId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Alerts");

            migrationBuilder.AlterColumn<int>(
                name: "MessageTypeId",
                table: "AlertInstances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AlertInstances_MessageTypes_MessageTypeId",
                table: "AlertInstances",
                column: "MessageTypeId",
                principalTable: "MessageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
