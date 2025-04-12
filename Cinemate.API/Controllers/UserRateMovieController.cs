using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRateMovieController : ControllerBase
    {
        private readonly IUserRateMovieService _userRateMovieService;

        public UserRateMovieController(IUserRateMovieService userRateMovieService)
        {
            _userRateMovieService = userRateMovieService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddUserRateMovie([FromBody] UserRateMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userRateMovieService.AddUserRateMovieAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return BadRequest(result); // Return 400 Bad Request with the error result
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserRateMovie([FromBody] UserRateMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userRateMovieService.DeleteUserRateMovieAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return NotFound(result); // Return 404 Not Found if the like was not found
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetRateWatchedMovies(CancellationToken cancellationToken)
        {
            var Rated = await _userRateMovieService.GetUserRateMoviesAsync(cancellationToken);

            if (Rated == null || !Rated.Any())
            {
                return NotFound("No Rated found."); // Return 404 if no likes are found
            }

            return Ok(Rated); // Return 200 OK with the list of liked movies
        }




    }
}
