using Cinemate.Core.Contracts.Profile;
using Cinemate.Core.Extensions;
using Cinemate.Core.Filters;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(CancellationToken cancellationToken)
        {
            var result = await _profileService.DeleteAsync(User.GetUserName()!, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromForm] UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var result = await _profileService.UpdateProfileAsync(request, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
        [HttpGet("LikedMovies")]
        public async Task<IActionResult> GetAllLikedMovies(CancellationToken cancellationToken)
        {
            var likedMovies = await _profileService.GetAllMoviesLiked(cancellationToken);
            return Ok(likedMovies);
        }
        [HttpGet("RatedMovies")]
        public async Task<IActionResult> GetAllRatedMovies(CancellationToken cancellationToken)
        {
            var ratedMovies = await _profileService.GetAllMoviesRated(cancellationToken);
            return Ok(ratedMovies);
        }
        [HttpGet("ReviewedMovies")]
        public async Task<IActionResult> GetAllReviewedMovies(CancellationToken cancellationToken)
        {
            var reviewedMovies = await _profileService.GetAllMoviesReviews(cancellationToken);
            return Ok(reviewedMovies);
        }
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
        [HttpGet("feed")]
		public async Task<IActionResult> GetUserFeed(CancellationToken cancellationToken)
		{
			var result = await _profileService.GetFeedForUserAsync(User.GetUserId()!, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}		[HttpGet("details/{userName}")]
		[XssProtection]
		public async Task<IActionResult> GetFollowCount([FromRoute] string userName, CancellationToken cancellationToken)
		{
			var result = await _profileService.GetUserDetailsAsync(userName, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		[HttpGet("RecentActivity/{userName}")]
		[XssProtection]
        public async Task<IActionResult> GetRecentActivity([FromRoute] string userName, CancellationToken cancellationToken)
        {
            var result = await _profileService.GetAllRecentActivity(userName, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
		[HttpPut("toggle-recent")]
		public async Task<IActionResult> ToggleRecentActivity(CancellationToken cancellationToken)
		{
			var result = await _profileService.ToggleRecentActivity(User.GetUserName()!, cancellationToken);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
		[HttpPut("toggle-following")]
		public async Task<IActionResult> ToggleFollowingAndFollowers(CancellationToken cancellationToken)
		{
			var result = await _profileService.ToggleFollowerAndFollowing(User.GetUserName()!, cancellationToken);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
		[HttpPut("toggle-notify-following")]
		public async Task<IActionResult> ToggleNotificationFollowing(CancellationToken cancellationToken)
		{
			var result = await _profileService.ToggleNotificationFollowing(User.GetUserName()!, cancellationToken);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
		[HttpPut("toggle-notify-new-release")]
		public async Task<IActionResult> ToggleNotificationNewRelease(CancellationToken cancellationToken)
		{
			var result = await _profileService.ToggleNotificationNewRelease(User.GetUserName()!, cancellationToken);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
		[HttpGet("privacy")]
        public async Task<IActionResult> GetPrivacy(CancellationToken cancellationToken)
        {
            var result = await _profileService.GetPrivacyAsync(User.GetUserName()!, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
		[HttpGet("notify-privacy")]
		public async Task<IActionResult> GetNotificationPrivacy(CancellationToken cancellationToken)
		{
			var result = await _profileService.GetNotificationPrivacy(User.GetUserName()!, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		[HttpGet("start-test")]
		public async Task<IActionResult> StartTest(CancellationToken cancellationToken)
		{
			var result = await _profileService.CalculateUserTestAsync(User.GetUserName()!, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		//[HttpPost("end-test")]
		//public async Task<IActionResult> EndTest([FromBody] List<MovieRatingItem> ratings, CancellationToken cancellationToken)
		//{
		//	var result = await _profileService.TestMLRecommendationFlowAsync(User.GetUserName()!, ratings, cancellationToken);
		//	return result.IsSuccess ? Ok(result) : result.ToProblem();
		//}
	}
}
