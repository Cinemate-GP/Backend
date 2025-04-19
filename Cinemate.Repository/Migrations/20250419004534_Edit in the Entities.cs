using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditintheEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeMovies_Movies_MovieId",
                table: "UserLikeMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieWatchList_Movies_MovieId",
                table: "UserMovieWatchList");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRateMovies_Movies_MovieId",
                table: "UserRateMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchedMovies_Movies_MovieId",
                table: "UserWatchedMovies");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "UserWatchedMovies",
                newName: "TMDBId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWatchedMovies_MovieId",
                table: "UserWatchedMovies",
                newName: "IX_UserWatchedMovies_TMDBId");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "UserRateMovies",
                newName: "TMDBId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRateMovies_MovieId",
                table: "UserRateMovies",
                newName: "IX_UserRateMovies_TMDBId");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "UserMovieWatchList",
                newName: "TMDBId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMovieWatchList_MovieId",
                table: "UserMovieWatchList",
                newName: "IX_UserMovieWatchList_TMDBId");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "UserLikeMovies",
                newName: "TMDBId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLikeMovies_MovieId",
                table: "UserLikeMovies",
                newName: "IX_UserLikeMovies_TMDBId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeMovies_Movies_TMDBId",
                table: "UserLikeMovies",
                column: "TMDBId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieWatchList_Movies_TMDBId",
                table: "UserMovieWatchList",
                column: "TMDBId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRateMovies_Movies_TMDBId",
                table: "UserRateMovies",
                column: "TMDBId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchedMovies_Movies_TMDBId",
                table: "UserWatchedMovies",
                column: "TMDBId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeMovies_Movies_TMDBId",
                table: "UserLikeMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieWatchList_Movies_TMDBId",
                table: "UserMovieWatchList");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRateMovies_Movies_TMDBId",
                table: "UserRateMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchedMovies_Movies_TMDBId",
                table: "UserWatchedMovies");

            migrationBuilder.RenameColumn(
                name: "TMDBId",
                table: "UserWatchedMovies",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserWatchedMovies_TMDBId",
                table: "UserWatchedMovies",
                newName: "IX_UserWatchedMovies_MovieId");

            migrationBuilder.RenameColumn(
                name: "TMDBId",
                table: "UserRateMovies",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRateMovies_TMDBId",
                table: "UserRateMovies",
                newName: "IX_UserRateMovies_MovieId");

            migrationBuilder.RenameColumn(
                name: "TMDBId",
                table: "UserMovieWatchList",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMovieWatchList_TMDBId",
                table: "UserMovieWatchList",
                newName: "IX_UserMovieWatchList_MovieId");

            migrationBuilder.RenameColumn(
                name: "TMDBId",
                table: "UserLikeMovies",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLikeMovies_TMDBId",
                table: "UserLikeMovies",
                newName: "IX_UserLikeMovies_MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeMovies_Movies_MovieId",
                table: "UserLikeMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieWatchList_Movies_MovieId",
                table: "UserMovieWatchList",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRateMovies_Movies_MovieId",
                table: "UserRateMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchedMovies_Movies_MovieId",
                table: "UserWatchedMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "TMDBId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
