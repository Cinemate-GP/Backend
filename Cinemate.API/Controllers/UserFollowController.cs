using Cinemate.Core.Contracts.Follow;
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
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserFollow([FromBody] AddFollowRequest request, CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.AddUserFollowAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserFollow([FromBody] AddFollowRequest request, CancellationToken cancellationToken)
        {
            var result = await _userFollowMovieService.DeleteUserFollowAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
		[HttpGet("get-all-followers/{userName}")]
		public async Task<IActionResult> GetAllUserFollowers([FromRoute] string userName, CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.GetAllFollowers(userName, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		[HttpGet("get-all-following/{userName}")]
		public async Task<IActionResult> GetAllUserFollowing([FromRoute] string userName, CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.GetAllFollowing(userName, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		[HttpGet("follow-details/{followName}")]
		public async Task<IActionResult> GetFollowDetails([FromRoute] string followName, CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.GetFollowersDetailsAsync(User.GetUserId()!, followName, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		[HttpDelete("remove-follower")]
		public async Task<IActionResult> GetFollowDetails([FromBody] RemoveFollowerRequest request, CancellationToken cancellationToken)
		{
			var result = await _userFollowMovieService.RemoveFollowersAsync(User.GetUserName()!, request, cancellationToken);
			return result.IsSuccess ? Ok() : result.ToProblem();
		}
	}
}
