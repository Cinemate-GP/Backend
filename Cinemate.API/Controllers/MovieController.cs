using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Repository.Abstractions;
using Cinemate.Core.Contracts.Movies;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
		public MovieController(IMovieService movieService)
		{
			_movieService = movieService;
		}
		[HttpGet("top-ten")]
		public async Task<IActionResult> GetTopTenMovies(CancellationToken cancellationToken)
		{ 
			var result = await _movieService.GetMovieTopTenAsync(cancellationToken);
			return Ok(result);
		}
		[HttpGet("{tmdbid}")]
		public async Task<IActionResult> GetMovieDetails(int tmdbid, CancellationToken cancellationToken)
		{
			var result = await _movieService.GetMovieDetailsAsync(tmdbid, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
		[HttpGet("random-movie")]
		public async Task<IActionResult> GetRandomMovie(CancellationToken cancellationToken)
		{
			var result = await _movieService.GetMovieRandomAsync(cancellationToken);
			return Ok(result);
		}
		[HttpGet("genre")]
		public async Task<IActionResult> GetGenera([FromQuery] MovieGeneraRequest? request, CancellationToken cancellationToken)
		{
			var result = await _movieService.GetMovieBasedOnGeneraAsync(request, cancellationToken);
			return Ok(result);
		}
	}
}
