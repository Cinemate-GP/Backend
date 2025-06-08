using Cinemate.Core.Contracts.User_Rate_Movie;
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
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserRateMovie([FromBody] UserRateMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userRateMovieService.DeleteUserRateMovieAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetRateWatchedMovies(CancellationToken cancellationToken)
        {
            var Rated = await _userRateMovieService.GetUserRateMoviesAsync(cancellationToken);
            return Rated == null || !Rated.Any() ? NotFound("No Rated found.") : Ok(Rated);
        }
		[HttpGet("")]
		public async Task<IActionResult> GetUserMoviesRated(CancellationToken cancellationToken)
		{
			var result = await _userRateMovieService.GetMoviesRatedByUserAsync(User.GetUserId()!, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
	}
}
