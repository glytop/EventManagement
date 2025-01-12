using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsWebApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToEventv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Events");
        }
    }
}
