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
using Cinemate.Core.Abstractions;
using Cinemate.Core.Contracts.Common;

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
					m.TMDBId,
					m.Title,
					m.PosterPath,
					m.IMDBRating,
					m.MPA
				)).ToList();

			return response;
		}
		public async Task<IEnumerable<MoviesTopTenResponse>> GetMovieTopTenRatedAsync(CancellationToken cancellationToken = default)
		{
			var movieRepository = _unitOfWork.Repository<Movie>();
			var allMovies = await movieRepository.GetAllAsync();
			var response = allMovies
				.Where(m => !string.IsNullOrEmpty(m.IMDBRating) && ((m.PosterPath != null) || (m.Trailer != null)))
				.OrderByDescending(m => ParseImdbRating(m.IMDBRating))
				.Take(10)
				.Select(m => new MoviesTopTenResponse(
					m.TMDBId,
					m.Title,
					m.PosterPath,
					m.IMDBRating,
					m.MPA
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
				.Include(m => m.UserReviews)
                .FirstOrDefaultAsync(m => m.TMDBId == tmdbid, cancellationToken);

			if (movie is null)
				return Result.Failure<MovieDetailsResponse>(MovieErrors.MovieNotFound);

			var actors = movie.CastMovies
				.Select(cm => new ActorMovieResponse(
					cm.Cast.CastId,
					cm.Cast.Name ?? string.Empty,
					cm.Cast.ProfilePath,
					cm.Role,
					cm.Extra
				)).ToList();

			var reviews = await _context.UserReviewMovies
				.Include(r => r.User)
				.Where(r => r.TMDBId == tmdbid)
				.Select(r => new MovieReviewResponse(
					r.UserId,
					r.TMDBId,
					r.User.FullName,
					r.User.ProfilePic,
					r.ReviewId,
					r.ReviewBody
				)).ToListAsync(cancellationToken);

			var genres = movie.MovieGenres
				.Select(mg => new GenresDetails(
					mg.Genre.Id,
					mg.Genre.Name ?? string.Empty
				)).ToList();

			var response = new MovieDetailsResponse(
				movie.TMDBId,
				movie.Title,
				movie.Overview,
				movie.Tagline,
				movie.PosterPath,
				movie.BackdropPath,
				movie.Language,
				movie.Runtime,
				movie.ReleaseDate,
				movie.Trailer,
				movie.IMDBRating,
				movie.RottenTomatoesRating,
				movie.MetacriticRating,
				movie.MPA,
				actors,
				genres,
				reviews
			);

			return Result.Success(response);
		}
		public async Task<IEnumerable<MovieDetailsRandomResponse>> GetMovieRandomAsync(CancellationToken cancellationToken = default)
		{
			var movieRepository = _unitOfWork.Repository<Movie>();
			var allMovies = await movieRepository.GetAllAsync();
			var top100Movies = allMovies
				.Where(m => m.Popularity != null && ((m.PosterPath != null) || (m.Trailer != null)))
				.OrderByDescending(m => m.Popularity)
				.Take(100)
				.ToList();

			var availableMovies = top100Movies.Where(m => !_returnedMovieIds.Contains(m.TMDBId)).ToList();
			var movieIds = availableMovies
				.OrderBy(_ => Guid.NewGuid())
				.Take(3)
				.Select(m => m.TMDBId)
				.ToList();

			foreach (var id in movieIds)
				_returnedMovieIds.Add(id);

			var randomMovies = await _context.Movies
				.Include(m => m.MovieGenres)
				.ThenInclude(mg => mg.Genre)
				.Where(m => movieIds.Contains(m.TMDBId))
				.ToListAsync(cancellationToken);

			return randomMovies.Select(m => new MovieDetailsRandomResponse(
				m.TMDBId,
				m.Title,
				m.Tagline,
				m.PosterPath,
				m.BackdropPath,
				m.IMDBRating,
				m.Runtime,
				m.ReleaseDate,
				m.Language,
				m.Trailer,
				m.MovieGenres.Select(mg => new GenresDetails(
					mg.Genre.Id,
					mg.Genre.Name!
				))
			)).ToList();
		}
		public async Task<IEnumerable<MoviesTopTenResponse>> GetMovieBasedOnGeneraAsync(MovieGeneraRequest? request, CancellationToken cancellationToken = default)
		{
			if (request == null || string.IsNullOrWhiteSpace(request.Genere))
			{
				var top100ByYear = await _context.Movies
					.Where(m => m.ReleaseDate != null)
					.OrderByDescending(m => m.ReleaseDate)
					.Take(100)
					.ToListAsync(cancellationToken);

				var randomMovies = top100ByYear
					.OrderBy(_ => Guid.NewGuid())
					.Take(10)
					.ToList();

				return randomMovies.Select(m => new MoviesTopTenResponse(
					m.TMDBId,
					m.Title,
					m.PosterPath,
					m.IMDBRating,
					m.MPA
				)).ToList();
			}
			var genre = await _context.Genres
				.FirstOrDefaultAsync(g => g.Name == request.Genere, cancellationToken);

			var top50MoviesByYear = await _context.Movies
				.Include(m => m.MovieGenres)
				.Where(m => m.MovieGenres.Any(mg => mg.GenreId == genre.Id) && m.ReleaseDate != null)
				.OrderByDescending(m => m.ReleaseDate)
				.Take(50)
				.ToListAsync(cancellationToken);

			var availableMovies = top50MoviesByYear
				.Where(m => !_returnedMovieIds.Contains(m.TMDBId))
				.ToList();

			var selectedMovies = availableMovies
				.OrderBy(_ => Guid.NewGuid())
				.Take(10)
				.ToList();

			foreach (var movie in selectedMovies)
				_returnedMovieIds.Add(movie.TMDBId);

			var response = selectedMovies.Select(m => new MoviesTopTenResponse(
				m.TMDBId,
				m.Title,
				m.PosterPath,
				m.IMDBRating,
				m.MPA
			)).ToList();
			return response;
		}
		public async Task<PaginatedList<MoviesTopTenResponse>> GetPaginatedMovieBasedAsync(RequestFilters request, CancellationToken cancellationToken = default)
		{
			var query = _context.Movies.AsQueryable();
			if (!string.IsNullOrEmpty(request.SearchValue))
				query = query.Where(m => m.Title!.Contains(request.SearchValue));

			if (!string.IsNullOrEmpty(request.MPA))
				query = query.Where(m => m.MPA!.Contains(request.MPA));

			if (!string.IsNullOrEmpty(request.Gener))
				query = query.Where(m => m.MovieGenres.Any(mg => mg.Genre.Name == request.Gener));

			if (!string.IsNullOrEmpty(request.Year))
				query = query.Where(m => m.ReleaseDate.HasValue && m.ReleaseDate.Value.Year.ToString().Contains(request.Year));

			query = request.SortDirection == SortDirection.Descending
				? query.OrderByDescending(m => m.Popularity)
				: query.OrderBy(m => m.Popularity);

			var movies = query
				   .Select(m => new MoviesTopTenResponse(
					   m.TMDBId,
					   m.Title,
					   m.PosterPath,
					   m.IMDBRating,
					   m.MPA
				   )).AsNoTracking();

			var response = await PaginatedList<MoviesTopTenResponse>.CreateAsync(
				movies,
				request.PageNumber,
				request.PageSize,
				cancellationToken
			);
			return response;
		}
		public async Task<Result<IEnumerable<SearchResponse>>> GetSearchForMovieActorUsersAsync(RequestSearch request, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(request.SearchValue))
				return Result.Success<IEnumerable<SearchResponse>>(new List<SearchResponse>());

			var searchTerm = request.SearchValue.Trim().ToLower();
			var movieResults = await _context.Movies
				.Where(m => m.Title != null && m.Title.ToLower().Contains(searchTerm) && ((m.PosterPath != null) || (m.Trailer != null)))
				.Select(m => new SearchResponse(
					m.TMDBId.ToString(), 
					m.Title!,
					m.PosterPath ?? string.Empty,
					"Movie"
				)).AsNoTracking().ToListAsync(cancellationToken);

			var actorResults = await _context.Casts
				.Where(c => c.Name != null && c.Name.ToLower().Contains(searchTerm))
				.Select(c => new SearchResponse(
					c.CastId.ToString(), 
					c.Name!,
					c.ProfilePath ?? string.Empty,
					"Actor" 
				)).AsNoTracking().ToListAsync(cancellationToken);

			var userResults = await _context.Users
				.Where(u => u.FullName.ToLower().Contains(searchTerm) && !u.IsDisabled)
				.Select(u => new SearchResponse(
					u.Id,
					u.FullName,
					u.ProfilePic ?? string.Empty,
					"User" 
				)).AsNoTracking().ToListAsync(cancellationToken);

			var combinedResults = movieResults
				.Concat(actorResults)
				.Concat(userResults)
				.ToList();

			return Result.Success<IEnumerable<SearchResponse>>(combinedResults);
		}
		private static double ParseImdbRating(string? rating)
		{
			if (string.IsNullOrEmpty(rating))
				return 0;

			var slashIndex = rating.IndexOf('/');
			if (slashIndex > 0)
			{
				var ratingValue = rating.Substring(0, slashIndex);
				if (double.TryParse(ratingValue, System.Globalization.NumberStyles.AllowDecimalPoint,
					System.Globalization.CultureInfo.InvariantCulture, out double result))
				{
					return result;
				}
			}
			return 0;
		}
	}
}
