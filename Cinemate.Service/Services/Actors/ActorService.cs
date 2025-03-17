using Cinemate.Core.Contracts.Actors;
using Cinemate.Core.Contracts.Movies;
using Cinemate.Core.Errors.Actor;
using Cinemate.Core.Errors.Movie;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Cinemate.Repository.Data.Contexts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Service.Services.Actors
{
    public class ActorService : IActorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _context;
		public ActorService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_context = context;
		}
		public async Task<Result<ActorDetailsResponse>> GetActorDetailsAsync(int actorid, CancellationToken cancellationToken = default)
		{
			var actor = await _context.Casts
				.Include(a => a.CastMovies)
				.ThenInclude(cm => cm.Movie)
				.FirstOrDefaultAsync(a => a.Id == actorid, cancellationToken);

			if (actor is null)
				return Result.Failure<ActorDetailsResponse>(ActorErrors.ActorNotFound);

			var movies = actor.CastMovies
				.Select(cm => new MoviesTopTenResponse(
					cm.Movie.MovieId,
					cm.Movie.TMDBId,
					cm.Movie.Title ?? string.Empty,
					cm.Movie.Poster_path
				))
				.ToList();

			var result = new ActorDetailsResponse(
				actor.Id,
				actor.Name ?? string.Empty,
				actor.ProfilePath,
				movies
			);

			return Result.Success(result);
		}
	}
}
