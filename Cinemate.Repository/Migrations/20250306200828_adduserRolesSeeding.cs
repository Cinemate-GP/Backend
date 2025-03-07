using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class adduserRolesSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e1940bc8-a54c-494d-9286-a585466c73f0", "0a20e01f-e5ea-4ce0-afbc-ac24742c2732" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e1940bc8-a54c-494d-9286-a585466c73f0", "0a20e01f-e5ea-4ce0-afbc-ac24742c2732" });
        }
    }
}
