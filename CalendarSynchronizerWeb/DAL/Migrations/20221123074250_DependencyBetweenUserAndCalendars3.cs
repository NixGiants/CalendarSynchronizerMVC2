using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarSynchronizerWeb.Migrations
{
    public partial class DependencyBetweenUserAndCalendars3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Calendars",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_AppUserId1",
                table: "Calendars",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_AspNetUsers_AppUserId1",
                table: "Calendars",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_AspNetUsers_AppUserId1",
                table: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_AppUserId1",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Calendars");
        }
    }
}
