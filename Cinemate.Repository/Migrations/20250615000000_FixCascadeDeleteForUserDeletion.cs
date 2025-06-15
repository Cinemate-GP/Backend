using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemate.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDeleteForUserDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing foreign key constraints for UserFollow table
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows");

            // Ensure UserLikeMovies has cascade delete for User relationship
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeMovies_AspNetUsers_UserId",
                table: "UserLikeMovies");

            // Ensure UserMovieWatchList has cascade delete for User relationship
            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieWatchList_AspNetUsers_UserId",
                table: "UserMovieWatchList");

            // Ensure UserRateMovies has cascade delete for User relationship
            migrationBuilder.DropForeignKey(
                name: "FK_UserRateMovies_AspNetUsers_UserId",
                table: "UserRateMovies");

            // Ensure UserReviewMovies has cascade delete for User relationship
            migrationBuilder.DropForeignKey(
                name: "FK_UserReviewMovies_AspNetUsers_UserId",
                table: "UserReviewMovies");

            // Ensure UserWatchedMovies has cascade delete for User relationship
            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchedMovies_AspNetUsers_UserId",
                table: "UserWatchedMovies");

            // Ensure UserCastFollows has cascade delete for User relationship
            migrationBuilder.DropForeignKey(
                name: "FK_UserCastFollows_AspNetUsers_UserId",
                table: "UserCastFollows");

            // Re-add foreign key constraints with cascade delete for user relationships
            
            // UserFollow - Cascade delete when the follower (UserId) is deleted, but restrict when followed user (FollowId) is deleted
            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowId",
                table: "UserFollows",
                column: "FollowId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // All other user-related entities should cascade delete when user is deleted
            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeMovies_AspNetUsers_UserId",
                table: "UserLikeMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieWatchList_AspNetUsers_UserId",
                table: "UserMovieWatchList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRateMovies_AspNetUsers_UserId",
                table: "UserRateMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviewMovies_AspNetUsers_UserId",
                table: "UserReviewMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchedMovies_AspNetUsers_UserId",
                table: "UserWatchedMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCastFollows_AspNetUsers_UserId",
                table: "UserCastFollows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert to previous state
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_UserId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikeMovies_AspNetUsers_UserId",
                table: "UserLikeMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieWatchList_AspNetUsers_UserId",
                table: "UserMovieWatchList");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRateMovies_AspNetUsers_UserId",
                table: "UserRateMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReviewMovies_AspNetUsers_UserId",
                table: "UserReviewMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWatchedMovies_AspNetUsers_UserId",
                table: "UserWatchedMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCastFollows_AspNetUsers_UserId",
                table: "UserCastFollows");

            // Restore original constraints
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

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikeMovies_AspNetUsers_UserId",
                table: "UserLikeMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieWatchList_AspNetUsers_UserId",
                table: "UserMovieWatchList",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRateMovies_AspNetUsers_UserId",
                table: "UserRateMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviewMovies_AspNetUsers_UserId",
                table: "UserReviewMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWatchedMovies_AspNetUsers_UserId",
                table: "UserWatchedMovies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCastFollows_AspNetUsers_UserId",
                table: "UserCastFollows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
