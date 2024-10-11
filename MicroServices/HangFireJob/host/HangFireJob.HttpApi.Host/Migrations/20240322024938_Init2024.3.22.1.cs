using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangFireJob.Migrations
{
    /// <inheritdoc />
    public partial class Init20243221 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastExecution",
                table: "Hangfire_HttpJob",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastJobState",
                table: "Hangfire_HttpJob",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastExecution",
                table: "Hangfire_HttpJob");

            migrationBuilder.DropColumn(
                name: "LastJobState",
                table: "Hangfire_HttpJob");
        }
    }
}
