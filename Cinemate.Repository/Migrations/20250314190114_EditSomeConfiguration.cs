using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditSomeConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenreMovie_Movies_CastsMovieId",
                table: "GenreMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenreMovie",
                table: "GenreMovie");

            migrationBuilder.DropIndex(
                name: "IX_GenreMovie_GenresId",
                table: "GenreMovie");

            migrationBuilder.RenameColumn(
                name: "trailer_path",
                table: "Movies",
                newName: "Trailer_path");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Movies",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "runtime",
                table: "Movies",
                newName: "Runtime");

            migrationBuilder.RenameColumn(
                name: "revenue",
                table: "Movies",
                newName: "Revenue");

            migrationBuilder.RenameColumn(
                name: "release_date",
                table: "Movies",
                newName: "Release_date");

            migrationBuilder.RenameColumn(
                name: "poster_path",
                table: "Movies",
                newName: "Poster_path");

            migrationBuilder.RenameColumn(
                name: "popularity",
                table: "Movies",
                newName: "Popularity");

            migrationBuilder.RenameColumn(
                name: "overview",
                table: "Movies",
                newName: "Overview");

            migrationBuilder.RenameColumn(
                name: "budget",
                table: "Movies",
                newName: "Budget");

            migrationBuilder.RenameColumn(
                name: "adult",
                table: "Movies",
                newName: "Adult");

            migrationBuilder.RenameColumn(
                name: "CastsMovieId",
                table: "GenreMovie",
                newName: "MoviesMovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenreMovie",
                table: "GenreMovie",
                columns: new[] { "GenresId", "MoviesMovieId" });

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovie_MoviesMovieId",
                table: "GenreMovie",
                column: "MoviesMovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_GenreMovie_Movies_MoviesMovieId",
                table: "GenreMovie",
                column: "MoviesMovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenreMovie_Movies_MoviesMovieId",
                table: "GenreMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenreMovie",
                table: "GenreMovie");

            migrationBuilder.DropIndex(
                name: "IX_GenreMovie_MoviesMovieId",
                table: "GenreMovie");

            migrationBuilder.RenameColumn(
                name: "Trailer_path",
                table: "Movies",
                newName: "trailer_path");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Movies",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Runtime",
                table: "Movies",
                newName: "runtime");

            migrationBuilder.RenameColumn(
                name: "Revenue",
                table: "Movies",
                newName: "revenue");

            migrationBuilder.RenameColumn(
                name: "Release_date",
                table: "Movies",
                newName: "release_date");

            migrationBuilder.RenameColumn(
                name: "Poster_path",
                table: "Movies",
                newName: "poster_path");

            migrationBuilder.RenameColumn(
                name: "Popularity",
                table: "Movies",
                newName: "popularity");

            migrationBuilder.RenameColumn(
                name: "Overview",
                table: "Movies",
                newName: "overview");

            migrationBuilder.RenameColumn(
                name: "Budget",
                table: "Movies",
                newName: "budget");

            migrationBuilder.RenameColumn(
                name: "Adult",
                table: "Movies",
                newName: "adult");

            migrationBuilder.RenameColumn(
                name: "MoviesMovieId",
                table: "GenreMovie",
                newName: "CastsMovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenreMovie",
                table: "GenreMovie",
                columns: new[] { "CastsMovieId", "GenresId" });

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovie_GenresId",
                table: "GenreMovie",
                column: "GenresId");

            migrationBuilder.AddForeignKey(
                name: "FK_GenreMovie_Movies_CastsMovieId",
                table: "GenreMovie",
                column: "CastsMovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
