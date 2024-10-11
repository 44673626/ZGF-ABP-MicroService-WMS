using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangFireJob.Migrations
{
    /// <inheritdoc />
    public partial class Init20243212 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateName",
                table: "Hangfire_HttpJob",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateName",
                table: "Hangfire_HttpJob");
        }
    }
}
