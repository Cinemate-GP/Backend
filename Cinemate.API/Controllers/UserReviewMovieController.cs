using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserReviewMovieController : ControllerBase
    {
        private readonly IUserReviewMovieService _userReviewMovieService;
        public UserReviewMovieController(IUserReviewMovieService userReviewMovieService)
        {
            _userReviewMovieService = userReviewMovieService;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserReviewMovie([FromBody] UserReviewMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userReviewMovieService.AddUserReviewMovieAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserReviewMovie([FromBody] UserReviewDeleteMovieResponse response, CancellationToken cancellationToken)
        {
            var result = await _userReviewMovieService.DeleteUserReviewMovieAsync(response, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetReviewWatchedMovies(CancellationToken cancellationToken)
        {
            var Review = await _userReviewMovieService.GetUserReviewMoviesAsync(cancellationToken);
            return Review == null || !Review.Any() ? NotFound("No Reviewed found.") : Ok(Review);
        }
    }
}
