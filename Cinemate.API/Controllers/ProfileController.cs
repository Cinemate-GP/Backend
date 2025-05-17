using Azure;
using Cinemate.Core.Contracts.Profile;
using Cinemate.Core.Extensions;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Repository.Abstractions;

namespace Cinemate.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        /// <summary>
        /// Delete the authenticated user's account and sign out.
        /// </summary>
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(CancellationToken cancellationToken)
        {
            var result = await _profileService.DeleteAsync(cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        /// <summary>
        /// Update the authenticated user's profile.
        /// </summary>
        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromForm] UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var updatedRequest = await _profileService.UpdateProfileAsync(request, cancellationToken);
            return Ok(updatedRequest);
        }

        /// <summary>
        /// Get all movies liked by the authenticated user.
        /// </summary>
        [HttpGet("LikedMovies")]
        public async Task<IActionResult> GetAllLikedMovies(CancellationToken cancellationToken)
        {
            var likedMovies = await _profileService.GetAllMoviesLiked(cancellationToken);
            return Ok(likedMovies);
        }

        /// <summary>
        /// Get all movies rated by the authenticated user.
        /// </summary>
        [HttpGet("RatedMovies")]
        public async Task<IActionResult> GetAllRatedMovies(CancellationToken cancellationToken)
        {
            var ratedMovies = await _profileService.GetAllMoviesRated(cancellationToken);
            return Ok(ratedMovies);
        }

        /// <summary>
        /// Get all movies reviewed by the authenticated user.
        /// </summary>
        [HttpGet("ReviewedMovies")]
        public async Task<IActionResult> GetAllReviewedMovies(CancellationToken cancellationToken)
        {
            var reviewedMovies = await _profileService.GetAllMoviesReviews(cancellationToken);
            return Ok(reviewedMovies);
        }

        /// <summary>
        /// Get all movies watched by the authenticated user.
        /// </summary>
        [HttpGet("WatchedMovies")]
        public async Task<IActionResult> GetAllWatchedMovies(CancellationToken cancellationToken)
        {
            var watchedMovies = await _profileService.GetAllMoviesWatched(cancellationToken);
            return Ok(watchedMovies);
        }
        [HttpGet("WatchlistMovies")]
        public async Task<IActionResult> GetAllWatchlistMovies(CancellationToken cancellationToken)
        {
            var watchedMovies = await _profileService.GetAllWatchlist(cancellationToken);
            return Ok(watchedMovies);
        }
        [HttpGet("CountFollowers")]
        public async Task<IActionResult> CountFollowers(CancellationToken cancellationToken)
        {
            var Following = await _profileService.CountFollowers(cancellationToken);
            return Ok(Following);
        }
        [HttpGet("CountFollowing")]
        public async Task<IActionResult> CountFollowing(CancellationToken cancellationToken)
        {
            var Following = await _profileService.CountFollowing(cancellationToken);
            return Ok(Following);
        }

        [HttpGet("feed")]
		public async Task<IActionResult> GetUserFeed(CancellationToken cancellationToken)
		{
			var result = await _profileService.GetFeedForUserAsync(User.GetUserId()!, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		[HttpGet("details/{userName}")]
		public async Task<IActionResult> GetFollowCount([FromRoute] string userName, CancellationToken cancellationToken)
		{
			var result = await _profileService.GetUserDetailsAsync(userName, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		
		[HttpGet("RecentActivity/{userName}")]
        public async Task<IActionResult> GetRecentActivity([FromRoute] string userName, CancellationToken cancellationToken)
        {
            var result = await _profileService.GetAllRecentActivity(userName, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
