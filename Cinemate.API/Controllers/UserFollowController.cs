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
        [HttpGet("get-all-followers/{userName}")]
        public async Task<IActionResult> GetAllUserFollowers([FromRoute]string userName, CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.GetAllFollowers(userName, cancellationToken);
            return Ok(result);
        }
        [HttpGet("get-all-following/{userName}")]
        public async Task<IActionResult> GetAllUserFollowing([FromRoute] string userName, CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.GetAllFollowing(userName, cancellationToken);
            return Ok(result);
        }
		[HttpGet("follow-details/{followName}")]
		public async Task<IActionResult> GetFollowDetails([FromRoute] string followName, CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.GetFollowersDetailsAsync(User.GetUserId()!, followName, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
	}
}
