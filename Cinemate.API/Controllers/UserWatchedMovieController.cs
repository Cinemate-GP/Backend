using Microsoft.AspNetCore.Authorization;
using Cinemate.Core.Contracts.User_Watched_Movie;
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
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserWatchedMovie([FromBody] UserWatchedMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userWatchedMovieService.DeleteUserWatchedMovieAsync(request, cancellationToken);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUserWatchedMovies(CancellationToken cancellationToken)
        {
            var Watched = await _userWatchedMovieService.GetUserWatchedMoviesAsync(cancellationToken);

            return Watched == null || !Watched.Any() ? NotFound("No Watched found.") : Ok(Watched); 
        }
    }
}
