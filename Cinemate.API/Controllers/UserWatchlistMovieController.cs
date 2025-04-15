using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWatchlistMovieController : ControllerBase
    {
        private readonly IUserWatchlistMovieService _userWatchlistMovieService;

        // Inject the service into the constructor
        public UserWatchlistMovieController(IUserWatchlistMovieService userWatchlistMovieService)
        {
            _userWatchlistMovieService = userWatchlistMovieService;
        }

        // POST: api/UserLikeMovie
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserWatchlistMovie([FromBody] UserWatchListMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userWatchlistMovieService.AddUserWatchlistMovieAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return BadRequest(result); // Return 400 Bad Request with the error result
        }

        // DELETE: api/UserLikeMovie/{id}
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserWatchlistMovie([FromBody] UserWatchListMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userWatchlistMovieService.DeleteUserWatchlistMovieAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return NotFound(result); // Return 404 Not Found if the like was not found
        }

        // GET: api/UserLikeMovie
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUserWatchlistMovies(CancellationToken cancellationToken)
        {
            var likes = await _userWatchlistMovieService.GetUserWatchlistMoviesAsync(cancellationToken);

            if (likes == null || !likes.Any())
            {
                return NotFound("No Watchlist found."); // Return 404 if no likes are found
            }

            return Ok(likes); // Return 200 OK with the list of liked movies
        }



    }
}
