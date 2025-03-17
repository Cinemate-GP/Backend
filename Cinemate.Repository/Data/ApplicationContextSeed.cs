using Cinemate.Core.Entities;
using Cinemate.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cinemate.Repository.Data
{
	public static class ApplicationContextSeed
	{
		private const int BatchSize = 100;
		public static async Task SeedAsync(ApplicationDbContext _context)
		{
			_context.ChangeTracker.AutoDetectChangesEnabled = false;

			try
			{
				if (!_context.Movies.Any())
				{
					var movieData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\Movie.json");
					var movies = JsonSerializer.Deserialize<List<Movie>>(movieData);
					if (movies?.Count() > 0)
					{
						await AddEntitiesInBatchesAsync(_context, movies);
						await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
					}
				}
				if (!_context.Genres.Any())
				{
					var genreData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\genres.json");
					var genres = JsonSerializer.Deserialize<List<Genre>>(genreData);
					if (genres?.Count() > 0)
					{
						await AddEntitiesInBatchesAsync(_context, genres);
						await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
					}
				}
				if (!_context.Casts.Any())
				{
					var castData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\MovieCasts.json");
					var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
					var casts = JsonSerializer.Deserialize<List<Cast>>(castData, jsonOptions);
					if (casts?.Count > 0)
					{
						await AddEntitiesInBatchesAsync(_context, casts);
						await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
					}
				}
				if (!_context.CastMovie.Any())
				{
					var movieIdMapping = await _context.Movies
						.GroupBy(m => m.TMDBId)
						.ToDictionaryAsync(g => g.Key, g => g.First().MovieId);

					var validCastIds = await _context.Casts.Select(c => c.Id).ToListAsync();
					var movieCastData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\MovieCastsLinks.json");
					var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
					var movieCasts = JsonSerializer.Deserialize<List<CastMovieData>>(movieCastData, jsonOptions);

					if (movieCasts?.Count > 0)
					{
						var validMovieCasts = movieCasts
							.Where(mc => movieIdMapping.ContainsKey(mc.tmdb_id) && validCastIds.Contains(mc.cast_id))
							.Select(mc => new CastMovie
							{
								Tmdb_Id = movieIdMapping[mc.tmdb_id], 
								CastId = mc.cast_id
							})
							.ToList();

						if (validMovieCasts.Any())
						{
							for (int i = 0; i < validMovieCasts.Count; i += BatchSize)
							{
								using var transaction = await _context.Database.BeginTransactionAsync();
								try
								{
									var batch = validMovieCasts.Skip(i).Take(BatchSize).ToList();
									await _context.CastMovie.AddRangeAsync(batch);
									await _context.SaveChangesAsync();
									await transaction.CommitAsync();
								}
								catch
								{
									await transaction.RollbackAsync();
									throw;
								}
								_context.ChangeTracker.Clear();
							}
							await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
						}
					}
				}
				if (!_context.MovieGenres.Any())
				{
					var validMovieIds = await _context.Movies.Select(m => m.MovieId).ToListAsync();
					var validGenreIds = await _context.Genres.Select(g => g.Id).ToListAsync();

					var movieGenreData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\movie_genres.json");
					var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
					var movieGenres = JsonSerializer.Deserialize<List<MovieGenreData>>(movieGenreData, jsonOptions);

					if (movieGenres?.Count() > 0)
					{
						var validMovieGenres = movieGenres
							.Where(mg => validMovieIds.Contains(mg.MovieId) && validGenreIds.Contains(mg.GenreId))
							.ToList();

						for (int i = 0; i < validMovieGenres.Count; i += BatchSize)
						{
							using var transaction = await _context.Database.BeginTransactionAsync();
							try
							{
								var batch = validMovieGenres.Skip(i).Take(BatchSize)
									.Select(mg => new MovieGenre
									{
										MovieId = mg.MovieId,
										GenreId = mg.GenreId
									})
									.ToList();

								await _context.MovieGenres.AddRangeAsync(batch);
								await _context.SaveChangesAsync();
								await transaction.CommitAsync();
							}
							catch
							{
								await transaction.RollbackAsync();
								throw;
							}
							_context.ChangeTracker.Clear();
						}
						await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
					}
				}
			}
			finally
			{
				_context.ChangeTracker.AutoDetectChangesEnabled = true;
			}
		}
		private static async Task AddEntitiesInBatchesAsync<TEntity>(ApplicationDbContext context, List<TEntity> entities) where TEntity : class
		{
			for (int i = 0; i < entities.Count; i += BatchSize)
			{
				using var transaction = await context.Database.BeginTransactionAsync();
				try
				{
					var batch = entities.Skip(i).Take(BatchSize).ToList();
					await context.Set<TEntity>().AddRangeAsync(batch);
					await context.SaveChangesAsync();
					await transaction.CommitAsync();
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
				context.ChangeTracker.Clear();
			}
		}
		private class CastMovieData
		{
			public int tmdb_id { get; set; }
			public int cast_id { get; set; }
		}
		private class MovieGenreData
		{
			public int MovieId { get; set; }
			public int GenreId { get; set; }
		}
	}
}
