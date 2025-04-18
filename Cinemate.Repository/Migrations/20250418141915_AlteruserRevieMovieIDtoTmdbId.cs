using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AlteruserRevieMovieIDtoTmdbId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReviewMovies_Movies_MovieId",
                table: "UserReviewMovies");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "UserReviewMovies",
                newName: "TMDBId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReviewMovies_MovieId",
                table: "UserReviewMovies",
                newName: "IX_UserReviewMovies_TMDBId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviewMovies_Movies_TMDBId",
                table: "UserReviewMovies",
                column: "TMDBId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReviewMovies_Movies_TMDBId",
                table: "UserReviewMovies");

            migrationBuilder.RenameColumn(
                name: "TMDBId",
                table: "UserReviewMovies",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReviewMovies_TMDBId",
                table: "UserReviewMovies",
                newName: "IX_UserReviewMovies_MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviewMovies_Movies_MovieId",
                table: "UserReviewMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
