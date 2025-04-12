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
    public class UserLikeMovieController : ControllerBase
    {
        private readonly IUserLikeMovieService _userLikeMovieService;

        // Inject the service into the constructor
        public UserLikeMovieController(IUserLikeMovieService userLikeMovieService)
        {
            _userLikeMovieService = userLikeMovieService;
        }

        // POST: api/UserLikeMovie
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserLikeMovie([FromBody] UserLikeMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userLikeMovieService.AddUserLikeMovieAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }

            return BadRequest(result); // Return 400 Bad Request with the error result
        }

        // DELETE: api/UserLikeMovie/{id}
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserLikeMovie([FromBody] UserLikeMovieResponse request, CancellationToken cancellationToken)
        {
            var result = await _userLikeMovieService.DeleteUserLikeMovieAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result); // Return 200 OK with the success result
            }                                   

            return NotFound(result); // Return 404 Not Found if the like was not found
        }

        // GET: api/UserLikeMovie
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUserLikeMovies(CancellationToken cancellationToken)
        {
            var likes = await _userLikeMovieService.GetUserLikeMoviesAsync(cancellationToken);

            if (likes == null || !likes.Any())
            {
                return NotFound("No likes found."); // Return 404 if no likes are found
            }

            return Ok(likes); // Return 200 OK with the list of liked movies
        }
    }
}
