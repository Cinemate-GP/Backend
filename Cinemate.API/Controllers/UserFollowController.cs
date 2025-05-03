using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Extensions;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Authorization;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFollowController : ControllerBase
    {
        private readonly IUserfollowService _userFollowMovieService;

        public UserFollowController(IUserfollowService userFollowMovieService)
        {
            _userFollowMovieService = userFollowMovieService;
        }

        // Inject the service into the constructor


        // POST: api/UserLikeMovie
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserFollow([FromBody] AddFollowRequest request, CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.AddUserFollowAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return BadRequest(result); // Return 400 Bad Request with the error result
        }

        // DELETE: api/UserLikeMovie/{id}
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserFollow([FromBody] AddFollowRequest request, CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.DeleteUserFollowAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return NotFound(result); // Return 404 Not Found if the like was not found
        }

        // GET: api/UserLikeMovie
        [HttpGet("get-all-followers/{userId}")]
        public async Task<IActionResult> GetAllUserFollowers([FromRoute]string userId, CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.GetAllFollowers(userId, cancellationToken);
            return Ok(result); // Return 200 OK with the list of liked movies
        }
        [HttpGet("get-all-following/{userId}")]
        public async Task<IActionResult> GetAllUserFollowing([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.GetAllFollowing(userId, cancellationToken);
            return Ok(result); // Return 200 OK with the list of liked movies
        }
		[HttpGet("count-follow/{userId}")]
		public async Task<IActionResult> GetFollowCount([FromRoute] string userId, CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.GetCountFollowersAndFollowingAsync(userId, cancellationToken);
			return result.IsSuccess ?  Ok(result.Value) : result.ToProblem();
		}
		[HttpGet("follow-details/{followId}")]
		public async Task<IActionResult> GetFollowDetails([FromRoute] string followId, CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.GetFollowersDetailsAsync(User.GetUserId()!, followId, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
	}
}
