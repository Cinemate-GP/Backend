using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivacyForActivityAndFollowing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnableFollowerAndFollowing",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnableRecentActivity",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0a20e01f-e5ea-4ce0-afbc-ac24742c2732",
                columns: new[] { "IsEnableFollowerAndFollowing", "IsEnableRecentActivity" },
                values: new object[] { false, false });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowId",
                table: "UserFollows",
                column: "FollowId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows");

            migrationBuilder.DropColumn(
                name: "IsEnableFollowerAndFollowing",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsEnableRecentActivity",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowId",
                table: "UserFollows",
                column: "FollowId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
