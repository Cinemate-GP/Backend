using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
	/// <inheritdoc />
	public partial class AddPrivactForNotification : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "IsEnableNotificationFollowing",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: true);

			migrationBuilder.AddColumn<bool>(
				name: "IsEnableNotificationNewRelease",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: true);

			migrationBuilder.CreateTable(
				name: "Notifications",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
					IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
					ActionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					NotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Notifications", x => x.Id);
					table.ForeignKey(
						name: "FK_Notifications_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			// Update the admin user with specific values for the notification settings
			migrationBuilder.UpdateData(
				table: "AspNetUsers",
				keyColumn: "Id",
				keyValue: "0a20e01f-e5ea-4ce0-afbc-ac24742c2732",
				columns: new[] { "IsEnableNotificationFollowing", "IsEnableNotificationNewRelease" },
				values: new object[] { false, false });

			migrationBuilder.CreateIndex(
				name: "IX_Notifications_UserId",
				table: "Notifications",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Notifications");

			migrationBuilder.DropColumn(
				name: "IsEnableNotificationFollowing",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "IsEnableNotificationNewRelease",
				table: "AspNetUsers");
		}
	}
}