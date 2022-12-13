using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarSynchronizerWeb.Migrations
{
    public partial class AddDatabaseScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalendarId",
                table: "Schedules",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    CalendarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.CalendarId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_CalendarId",
                table: "Schedules",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Calendars_CalendarId",
                table: "Schedules",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "CalendarId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Calendars_CalendarId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_CalendarId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "CalendarId",
                table: "Schedules");
        }
    }
}
