using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemate.Core.Service_Contract;
using Cinemate.Core.Contracts.Movies;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Entities;
using Cinemate.Core.Errors.Movie;
using Cinemate.Repository.Abstractions;
using MapsterMapper;
using Cinemate.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Cinemate.Core.Contracts.Actors;
using Cinemate.Core.Contracts.Genres;

namespace Cinemate.Service.Services.Movies
{
	public class MovieService : IMovieService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _context;
		private static readonly HashSet<int> _returnedMovieIds = new();
		public MovieService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_context = context;
		}
		public async Task<IEnumerable<MoviesTopTenResponse>> GetMovieTopTenAsync(CancellationToken cancellationToken = default)
		{
			var movieRepository = _unitOfWork.Repository<Movie>();
			var allMovies = await movieRepository.GetAllAsync();
			var response = allMovies
				.Where(m => m.Popularity != null)
				.OrderByDescending(m => m.Popularity)
				.Take(10)
				.Select(m => new MoviesTopTenResponse(
					m.MovieId,
					m.TMDBId,
					m.Title,
					m.Poster_path
				)).ToList();

			return response;
		}
		public async Task<Result<MovieDetailsResponse>> GetMovieDetailsAsync(int tmdbid, CancellationToken cancellationToken = default)
		{
			var movie = await _context.Movies
				.Include(m => m.CastMovies)
				.ThenInclude(cm => cm.Cast)
				.Include(m => m.MovieGenres)
				.ThenInclude(mg => mg.Genre)
				.FirstOrDefaultAsync(m => m.TMDBId == tmdbid, cancellationToken);

			if (movie is null)
				return Result.Failure<MovieDetailsResponse>(MovieErrors.MovieNotFound);

			var actors = movie.CastMovies
				.Select(cm => new ActorMovieResponse(
					cm.Cast.Id,
					cm.Cast.Name ?? string.Empty,
					cm.Cast.ProfilePath,
					cm.Cast.KnownForDepartment,
					cm.Cast.Character
				)).ToList();

			var genres = movie.MovieGenres
				.Select(mg => new GenresDetails(
					mg.Genre.Id,
					mg.Genre.Name ?? string.Empty
				)).ToList();

			var response = new MovieDetailsResponse(
				movie.MovieId,
				movie.TMDBId,
				movie.Title,
				movie.Overview,
				movie.Poster_path,
				movie.Runtime,
				movie.Release_date,
				movie.Trailer_path,
				actors,  
				genres
			);

			return Result.Success(response);
		}
		public async Task<IEnumerable<MovieDetailsRandomResponse>> GetMovieRandomAsync(CancellationToken cancellationToken = default)
		{
			var movieRepository = _unitOfWork.Repository<Movie>();
			var allMovies = await movieRepository.GetAllAsync();
			var movieIds = allMovies
					.OrderBy(_ => Guid.NewGuid())
					.Take(3)
					.Select(m => m.MovieId)
					.ToList();

			var randomMovies = await _context.Movies
					.Include(m => m.MovieGenres)
					.ThenInclude(mg => mg.Genre)
					.Where(m => movieIds.Contains(m.MovieId))
					.ToListAsync(cancellationToken);

			return randomMovies.Select(m => new MovieDetailsRandomResponse(
				   m.MovieId,
				   m.TMDBId,
				   m.Title,
				   m.Overview,
				   m.Poster_path,
				   m.Runtime,
				   m.Release_date,
				   m.Trailer_path,
				   m.MovieGenres.Select(mg => new GenresDetails(
					   mg.Genre.Id,
					   mg.Genre.Name ?? string.Empty
				   ))
			)).ToList();
		}
		public async Task<IEnumerable<MoviesTopTenResponse>> GetMovieBasedOnGeneraAsync(MovieGeneraRequest? request, CancellationToken cancellationToken = default)
		{
			if (request == null || string.IsNullOrWhiteSpace(request.Genere))
			{
				var genres = await _context.Genres.ToListAsync(cancellationToken);
				if (!genres.Any())
					return Enumerable.Empty<MoviesTopTenResponse>();

				var randomGenre = genres.OrderBy(_ => Guid.NewGuid()).First();
				var randomMovies = await _context.Movies
					.Include(m => m.MovieGenres)
					.Where(m => m.MovieGenres.Any(mg => mg.GenreId == randomGenre.Id))
					.OrderBy(_ => Guid.NewGuid())
					.Take(10)
					.ToListAsync(cancellationToken);

				return randomMovies.Select(m => new MoviesTopTenResponse(
					m.MovieId,
					m.TMDBId,
					m.Title,
					m.Poster_path
				)).ToList();
			}
			var genre = await _context.Genres
				.FirstOrDefaultAsync(g => g.Name == request.Genere, cancellationToken);

			if (genre == null)
				return Enumerable.Empty<MoviesTopTenResponse>();

			var availableMovies = await _context.Movies
				.Include(m => m.MovieGenres)
				.Where(m => m.MovieGenres.Any(mg => mg.GenreId == genre.Id) &&
						   !_returnedMovieIds.Contains(m.MovieId))
				.ToListAsync(cancellationToken);

			if (!availableMovies.Any())
			{
				_returnedMovieIds.Clear();

				availableMovies = await _context.Movies
					.Include(m => m.MovieGenres)
					.Where(m => m.MovieGenres.Any(mg => mg.GenreId == genre.Id))
					.ToListAsync(cancellationToken);
			}
			var randomizedMovies = availableMovies
				.OrderBy(_ => Guid.NewGuid())
				.Take(10)
				.ToList();

			foreach (var movie in randomizedMovies)
				_returnedMovieIds.Add(movie.MovieId);

			var response = randomizedMovies.Select(m => new MoviesTopTenResponse(
				m.MovieId,
				m.TMDBId,
				m.Title,
				m.Poster_path
			)).ToList();

			return response;
		}
	}
}
