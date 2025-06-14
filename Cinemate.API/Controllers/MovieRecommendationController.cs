
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieRecommendationController : ControllerBase
    {
        private readonly IMovieRecommendationService _recommendationService;

        public MovieRecommendationController(IMovieRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpPost("GetRecommendations")]
        public async Task<IActionResult> GetRecommendations(
            [FromBody] MovieRecommendationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _recommendationService.GetRecommendedMoviesAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}