using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticalWork.Reports.Data.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventDateOfActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_EventDate",
                table: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_EventType",
                table: "ActivityLogs");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EventDate",
                table: "ActivityLogs",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EventDate_EventType",
                table: "ActivityLogs",
                columns: new[] { "EventDate", "EventType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_EventDate_EventType",
                table: "ActivityLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "ActivityLogs",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EventDate",
                table: "ActivityLogs",
                column: "EventDate");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EventType",
                table: "ActivityLogs",
                column: "EventType");
        }
    }
}
