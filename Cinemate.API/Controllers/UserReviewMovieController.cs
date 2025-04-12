using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return BadRequest(result); // Return 400 Bad Request with the error result
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserReviewMovie([FromBody] UserReviewDeleteMovieResponse response, CancellationToken cancellationToken)
        {
            var result = await _userReviewMovieService.DeleteUserReviewMovieAsync(response, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return NotFound(result); // Return 404 Not Found if the like was not found
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetReviewWatchedMovies(CancellationToken cancellationToken)
        {
            var Review = await _userReviewMovieService.GetUserReviewMoviesAsync(cancellationToken);

            if (Review == null || !Review.Any())
            {
                return NotFound("No Reviewed found."); // Return 404 if no review are found
            }

            return Ok(Review); // Return 200 OK with the list of liked movies
        }












    }
}
