using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangFireJob.Migrations
{
    /// <inheritdoc />
    public partial class Init20243211 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Hangfire_HttpJob",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Hangfire_HttpJob");
        }
    }
}
