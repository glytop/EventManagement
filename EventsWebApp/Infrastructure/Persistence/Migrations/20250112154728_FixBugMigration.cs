using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsWebApp.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixBugMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Events_EventId1",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_EventId1",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "EventId1",
                table: "Participants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId1",
                table: "Participants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EventId1",
                table: "Participants",
                column: "EventId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Events_EventId1",
                table: "Participants",
                column: "EventId1",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
