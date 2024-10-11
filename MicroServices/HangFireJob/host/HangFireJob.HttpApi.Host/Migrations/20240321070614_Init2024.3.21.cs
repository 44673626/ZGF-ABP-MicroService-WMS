using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HangFireJob.Migrations
{
    /// <inheritdoc />
    public partial class Init2024321 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hangfire_HttpJob",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HttpUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobParameter = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Cron = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DelayInMinute = table.Column<double>(type: "float", nullable: false),
                    TimeSpanFromSeconds = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hangfire_HttpJob", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hangfire_HttpJob");
        }
    }
}
