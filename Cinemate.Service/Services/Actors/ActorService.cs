using Cinemate.Core.Contracts.Actors;
using Cinemate.Core.Contracts.Movies;
using Cinemate.Core.Errors.Actor;
using Cinemate.Core.Errors.Movie;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Cinemate.Repository.Data.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Cinemate.Service.Services.Actors
{
	public class ActorService : IActorService
	{
		private readonly ApplicationDbContext _context;
		public ActorService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<Result<ActorDetailsResponse>> GetActorDetailsAsync(int actorid, CancellationToken cancellationToken = default)
		{
			var actor = await _context.Casts
				.Include(a => a.CastMovies)
				.ThenInclude(cm => cm.Movie)
				.FirstOrDefaultAsync(a => a.CastId == actorid, cancellationToken);

			if (actor is null)
				return Result.Failure<ActorDetailsResponse>(ActorErrors.ActorNotFound);

			var actorDetails = actor.Adapt<ActorDetailsResponse>();
			var movies = actor.CastMovies
				.OrderByDescending(cm => cm.Movie.ReleaseDate)
				.Where(m => m.Movie.IsDeleted != true && m.Movie.PosterPath != null && m.Movie.Trailer != null)
				.Select(cm => cm.Movie.Adapt<MoviesTopTenResponse>())
				.ToList();

			var result = actorDetails with { Movies = movies };

			return Result.Success(result);
		}
	}
}
