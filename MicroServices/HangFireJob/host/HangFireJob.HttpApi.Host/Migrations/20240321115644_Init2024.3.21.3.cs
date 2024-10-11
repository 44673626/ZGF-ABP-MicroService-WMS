using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangFireJob.Migrations
{
    /// <inheritdoc />
    public partial class Init20243213 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Hangfire_HttpJob",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Hangfire_HttpJob");
        }
    }
}
