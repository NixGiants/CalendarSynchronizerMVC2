using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarSynchronizerWeb.Migrations
{
    public partial class DependencyBetweenUserAndCalendars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Calendars",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Calendars",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_AppUserId",
                table: "Calendars",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_AspNetUsers_AppUserId",
                table: "Calendars",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_AspNetUsers_AppUserId",
                table: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_AppUserId",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Calendars",
                newName: "UserId");
        }
    }
}
