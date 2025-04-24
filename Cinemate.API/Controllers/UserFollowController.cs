using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpGet("GetAllUserFollowers{Userid}")]
        public async Task<IActionResult> GetAllUserFollowers( string Userid,CancellationToken cancellationToken)
        {
            var Followers = await _userFollowMovieService.GetAllFollowers(Userid, cancellationToken);

            if (Followers == null || !Followers.Any())
            {
                return NotFound("No Followers found."); // Return 404 if no likes are found
            }

            return Ok(Followers); // Return 200 OK with the list of liked movies
        }
        [HttpGet("GetAllUserFollowing{Userid}")]
        public async Task<IActionResult> GetAllUserFollowing(string Userid, CancellationToken cancellationToken)
        {
            var Following = await _userFollowMovieService.GetAllFollowing(Userid, cancellationToken);

            if (Following == null || !Following.Any())
            {
                return NotFound("No Following found."); // Return 404 if no likes are found
            }

            return Ok(Following); // Return 200 OK with the list of liked movies
        }


















    }
}
