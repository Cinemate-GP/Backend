using Cinemate.Core.Service_Contract;
using Cinemate.Service.Services.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Repository.Abstractions;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
		private readonly IActorService _actorService;
		public ActorController(IActorService actorService)
		{
			_actorService = actorService;
		}
		[HttpGet("{actorid}")]
		public async Task<IActionResult> GetMovieDetails(int actorid, CancellationToken cancellationToken)
		{
			var result = await _actorService.GetActorDetailsAsync(actorid, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
	}
}
