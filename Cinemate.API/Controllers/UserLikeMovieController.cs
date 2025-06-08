using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserLikeMovieController : ControllerBase
    {
        private readonly IUserLikeMovieService _userLikeMovieService;
        public UserLikeMovieController(IUserLikeMovieService userLikeMovieService)
        {
            _userLikeMovieService = userLikeMovieService;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserLikeMovie([FromBody] UserLikeMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userLikeMovieService.AddUserLikeMovieAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserLikeMovie([FromBody] UserLikeMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userLikeMovieService.DeleteUserLikeMovieAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUserLikeMovies(CancellationToken cancellationToken)
        {
            var likes = await _userLikeMovieService.GetUserLikeMoviesAsync(cancellationToken);
            return likes == null || !likes.Any() ? NotFound("No likes found.") : Ok(likes);
        }
    }
}
