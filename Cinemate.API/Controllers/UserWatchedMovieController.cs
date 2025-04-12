using Microsoft.AspNetCore.Authorization;
using Cinemate.Core.Contracts.User_Watched_Movie;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Core.Service_Contract;


namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserWatchedMovieController : ControllerBase
    {
        private readonly IUserWatchedMovieService _userWatchedMovieService;

        public UserWatchedMovieController(IUserWatchedMovieService userWatchedMovieService)
        {
            _userWatchedMovieService = userWatchedMovieService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddUserWatchedMovie([FromBody] UserWatchedMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userWatchedMovieService.AddUserWatchedMovieAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return BadRequest(result); // Return 400 Bad Request with the error result
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserWatchedMovie([FromBody] UserWatchedMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userWatchedMovieService.DeleteUserWatchedMovieAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return NotFound(result); // Return 404 Not Found if the like was not found
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUserWatchedMovies(CancellationToken cancellationToken)
        {
            var Watched = await _userWatchedMovieService.GetUserWatchedMoviesAsync(cancellationToken);

            if (Watched == null || !Watched.Any())
            {
                return NotFound("No Watched found."); // Return 404 if no likes are found
            }

            return Ok(Watched); // Return 200 OK with the list of liked movies
        }





    }
}
