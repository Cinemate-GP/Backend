using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddLogosCloumForMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Movies");
        }
    }
}
