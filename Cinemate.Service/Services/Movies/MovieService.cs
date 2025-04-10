﻿using System;
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
			var top100Movies = allMovies
				.Where(m => m.Popularity != null)
				.OrderByDescending(m => m.Popularity)
				.Take(100)
				.ToList();

			var availableMovies = top100Movies.Where(m => !_returnedMovieIds.Contains(m.MovieId)).ToList();
			var movieIds = availableMovies
				.OrderBy(_ => Guid.NewGuid())
				.Take(3)
				.Select(m => m.MovieId)
				.ToList();

			foreach (var id in movieIds)
				_returnedMovieIds.Add(id);

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
				var top100ByYear = await _context.Movies
					.Where(m => m.Release_date != null)
					.OrderByDescending(m => m.Release_date)
					.Take(100)
					.ToListAsync(cancellationToken);

				var randomMovies = top100ByYear
					.OrderBy(_ => Guid.NewGuid())
					.Take(10)
					.ToList();

				return randomMovies.Select(m => new MoviesTopTenResponse(
					m.MovieId,
					m.TMDBId,
					m.Title,
					m.Poster_path
				)).ToList();
			}
			var genre = await _context.Genres
				.FirstOrDefaultAsync(g => g.Name == request.Genere, cancellationToken);

			var top50MoviesByYear = await _context.Movies
				.Include(m => m.MovieGenres)
				.Where(m => m.MovieGenres.Any(mg => mg.GenreId == genre.Id) && m.Release_date != null)
				.OrderByDescending(m => m.Release_date)
				.Take(50)
				.ToListAsync(cancellationToken);

			var availableMovies = top50MoviesByYear
				.Where(m => !_returnedMovieIds.Contains(m.MovieId))
				.ToList();

			var selectedMovies = availableMovies
				.OrderBy(_ => Guid.NewGuid())
				.Take(10)
				.ToList();

			foreach (var movie in selectedMovies)
				_returnedMovieIds.Add(movie.MovieId);

			var response = selectedMovies.Select(m => new MoviesTopTenResponse(
				m.MovieId,
				m.TMDBId,
				m.Title,
				m.Poster_path
			)).ToList();
			return response;
		}
		public async Task<PaginatedList<MoviesTopTenResponse>> GetPaginatedMovieBasedAsync(RequestFilters request, CancellationToken cancellationToken = default)
		{
			var query = _context.Movies.AsQueryable();
			if (!string.IsNullOrEmpty(request.SearchValue))
				query = query.Where(m => m.Title.Contains(request.SearchValue));

			if (!string.IsNullOrEmpty(request.Gener))
				query = query.Where(m => m.MovieGenres.Any(mg => mg.Genre.Name == request.Gener));

			if (!string.IsNullOrEmpty(request.Year))
				query = query.Where(m => m.Release_date.HasValue && m.Release_date.Value.Year.ToString().Contains(request.Year));

			query = request.SortDirection == SortDirection.Descending
				? query.OrderByDescending(m => m.Popularity)
				: query.OrderBy(m => m.Popularity);

			var movies = query
				   .Select(m => new MoviesTopTenResponse(
					   m.MovieId,
					   m.TMDBId,
					   m.Title,
					   m.Poster_path
				   )).AsNoTracking();

			var response = await PaginatedList<MoviesTopTenResponse>.CreateAsync(
				movies,
				request.PageNumber,
				request.PageSize,
				cancellationToken
			);
			return response;
		}
	}
}
