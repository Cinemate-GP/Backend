using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdentityForCastId : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "UsernameIndex",
				table: "AspNetUsers");

			migrationBuilder.DropIndex(
				name: "RolenameIndex",
				table: "AspNetRoles");

			migrationBuilder.RenameColumn(
				name: "name",
				table: "Genres",
				newName: "Name");

			migrationBuilder.RenameColumn(
				name: "name",
				table: "Casts",
				newName: "Name");

			migrationBuilder.RenameColumn(
				name: "name",
				table: "AspNetUserTokens",
				newName: "Name");

			migrationBuilder.RenameColumn(
				name: "Username",
				table: "AspNetUsers",
				newName: "UserName");

			migrationBuilder.RenameColumn(
				name: "NormalizedUsername",
				table: "AspNetUsers",
				newName: "NormalizedUserName");

			migrationBuilder.RenameColumn(
				name: "Fullname",
				table: "AspNetUsers",
				newName: "FullName");

			migrationBuilder.RenameColumn(
				name: "ProviderDisplayname",
				table: "AspNetUserLogins",
				newName: "ProviderDisplayName");

			migrationBuilder.RenameColumn(
				name: "name",
				table: "AspNetRoles",
				newName: "Name");

			migrationBuilder.RenameColumn(
				name: "Normalizedname",
				table: "AspNetRoles",
				newName: "NormalizedName");

			migrationBuilder.DropForeignKey(
				name: "FK_CastMovie_Casts_CastId",
				table: "CastMovie");

			migrationBuilder.DropForeignKey(
				name: "FK_UserCastFollows_Casts_CastId",
				table: "UserCastFollows");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Casts",
				table: "Casts");

			migrationBuilder.DropColumn(
				name: "Id",
				table: "Casts");

			migrationBuilder.AddColumn<int>(
				name: "Id",
				table: "Casts",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddPrimaryKey(
				name: "PK_Casts",
				table: "Casts",
				column: "Id");

			// Re-add foreign key constraints
			migrationBuilder.AddForeignKey(
				name: "FK_CastMovie_Casts_CastId",
				table: "CastMovie",
				column: "CastId",
				principalTable: "Casts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_UserCastFollows_Casts_CastId",
				table: "UserCastFollows",
				column: "CastId",
				principalTable: "Casts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "UserNameIndex",
				table: "AspNetUsers");

			migrationBuilder.DropIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles");

			migrationBuilder.RenameColumn(
				name: "Name",
				table: "Genres",
				newName: "name");

			migrationBuilder.RenameColumn(
				name: "Name",
				table: "Casts",
				newName: "name");

			migrationBuilder.RenameColumn(
				name: "Name",
				table: "AspNetUserTokens",
				newName: "name");

			migrationBuilder.RenameColumn(
				name: "UserName",
				table: "AspNetUsers",
				newName: "Username");

			migrationBuilder.RenameColumn(
				name: "NormalizedUserName",
				table: "AspNetUsers",
				newName: "NormalizedUsername");

			migrationBuilder.RenameColumn(
				name: "FullName",
				table: "AspNetUsers",
				newName: "Fullname");

			migrationBuilder.RenameColumn(
				name: "ProviderDisplayName",
				table: "AspNetUserLogins",
				newName: "ProviderDisplayname");

			migrationBuilder.RenameColumn(
				name: "NormalizedName",
				table: "AspNetRoles",
				newName: "Normalizedname");

			migrationBuilder.RenameColumn(
				name: "Name",
				table: "AspNetRoles",
				newName: "name");

			migrationBuilder.DropForeignKey(
				name: "FK_CastMovie_Casts_CastId",
				table: "CastMovie");

			migrationBuilder.DropForeignKey(
				name: "FK_UserCastFollows_Casts_CastId",
				table: "UserCastFollows");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Casts",
				table: "Casts");

			migrationBuilder.DropColumn(
				name: "Id",
				table: "Casts");

			migrationBuilder.AddColumn<int>(
				name: "Id",
				table: "Casts",
				type: "int",
				nullable: false,
				defaultValue: 0)
				.Annotation("SqlServer:Identity", "1, 1");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Casts",
				table: "Casts",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_CastMovie_Casts_CastId",
				table: "CastMovie",
				column: "CastId",
				principalTable: "Casts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_UserCastFollows_Casts_CastId",
				table: "UserCastFollows",
				column: "CastId",
				principalTable: "Casts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.CreateIndex(
				name: "UsernameIndex",
				table: "AspNetUsers",
				column: "NormalizedUsername",
				unique: true,
				filter: "[NormalizedUsername] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "RolenameIndex",
				table: "AspNetRoles",
				column: "Normalizedname",
				unique: true,
				filter: "[Normalizedname] IS NOT NULL");
		}
	}
}
