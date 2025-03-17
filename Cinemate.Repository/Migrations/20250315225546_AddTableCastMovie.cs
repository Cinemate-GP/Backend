using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddTableCastMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CastMovie_Casts_CastsId",
                table: "CastMovie");

            migrationBuilder.DropForeignKey(
				name: "FK_CastMovie_Movies_MoviesMovieId",
                table: "CastMovie");

            migrationBuilder.RenameColumn(
				name: "MoviesMovieId",
                table: "CastMovie",
                newName: "Tmdb_Id");

            migrationBuilder.RenameColumn(
				name: "CastsId",
                table: "CastMovie",
                newName: "CastId");

            migrationBuilder.RenameIndex(
                name: "IX_CastMovie_MoviesMovieId",
                table: "CastMovie",
                newName: "IX_CastMovie_Tmdb_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CastMovie_Casts_CastId",
                table: "CastMovie",
                column: "CastId",
                principalTable: "Casts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CastMovie_Movies_Tmdb_Id",
                table: "CastMovie",
                column: "Tmdb_Id",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CastMovie_Casts_CastId",
                table: "CastMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_CastMovie_Movies_Tmdb_Id",
                table: "CastMovie");

            migrationBuilder.RenameColumn(
				name: "Tmdb_Id",
                table: "CastMovie",
                newName: "MoviesMovieId");

            migrationBuilder.RenameColumn(
				name: "CastId",
                table: "CastMovie",
                newName: "CastsId");

            migrationBuilder.RenameIndex(
                name: "IX_CastMovie_Tmdb_Id",
                table: "CastMovie",
                newName: "IX_CastMovie_MoviesMovieId");

            migrationBuilder.AddForeignKey(
				name: "FK_CastMovie_Casts_CastsId",
                table: "CastMovie",
                column: "CastsId",
                principalTable: "Casts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
				name: "FK_CastMovie_Movies_MoviesMovieId",
                table: "CastMovie",
                column: "MoviesMovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
