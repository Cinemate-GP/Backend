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
        [HttpGet("get-all-followers")]
        public async Task<IActionResult> GetAllUserFollowers(CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.GetAllFollowers(User.GetUserId()!, cancellationToken);
            return Ok(result); // Return 200 OK with the list of liked movies
        }
        [HttpGet("get-all-following")]
        public async Task<IActionResult> GetAllUserFollowing(CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.GetAllFollowing(User.GetUserId()!, cancellationToken);
            return Ok(result); // Return 200 OK with the list of liked movies
        }
		[HttpGet("count-follow")]
		public async Task<IActionResult> GetFollowCount(CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.GetCountFollowersAndFollowingAsync(User.GetUserId()!, cancellationToken);
			return result.IsSuccess ?  Ok(result.Value) : result.ToProblem();
		}
		[HttpPost("follow-details")]
		public async Task<IActionResult> GetFollowDetails([FromBody] FollowerIdRequest request, CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.GetFollowersDetailsAsync(User.GetUserId()!, request, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
	}
}
