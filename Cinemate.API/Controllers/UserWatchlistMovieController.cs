using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Mvc;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWatchlistMovieController : ControllerBase
    {
        private readonly IUserWatchlistMovieService _userWatchlistMovieService;
        public UserWatchlistMovieController(IUserWatchlistMovieService userWatchlistMovieService)
        {
            _userWatchlistMovieService = userWatchlistMovieService;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserWatchlistMovie([FromBody] UserWatchListMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userWatchlistMovieService.AddUserWatchlistMovieAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserWatchlistMovie([FromBody] UserWatchListMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userWatchlistMovieService.DeleteUserWatchlistMovieAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUserWatchlistMovies(CancellationToken cancellationToken)
        {
            var likes = await _userWatchlistMovieService.GetUserWatchlistMoviesAsync(cancellationToken);
            return likes == null || !likes.Any() ? NotFound("No Watchlist found.") : Ok(likes);
        }
    }
}
