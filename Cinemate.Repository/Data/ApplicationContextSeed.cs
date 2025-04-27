//using Cinemate.Core.Entities;
//using Cinemate.Repository.Data.Contexts;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace Cinemate.Repository.Data
//{
//	public static class ApplicationContextSeed
//	{
//		private const int BatchSize = 100;
//		public static async Task SeedAsync(ApplicationDbContext _context)
//		{
//			_context.ChangeTracker.AutoDetectChangesEnabled = false;

//			try
//			{
//				if (!_context.Movies.Any())
//				{
//					var movieData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\movie_details.json");
//					var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//					var movieDetails = JsonSerializer.Deserialize<List<MovieData>>(movieData, jsonOptions);

//					if (movieDetails?.Count > 0)
//					{
//						var movies = movieDetails.Select(md => new Movie
//						{
//							TMDBId = int.Parse(md.tmdb_id),
//							IMDBId = md.imdb_id,
//							Title = md.title,
//							Overview = md.overview,
//							ReleaseDate = !string.IsNullOrEmpty(md.release_date) ? DateOnly.Parse(md.release_date) : null,
//							Runtime = md.runtime,
//							Language = md.language,
//							PosterPath = md.poster,
//							BackdropPath = md.backdrop,
//							Trailer = md.trailer,
//							Budget = md.budget,
//							Revenue = md.revenue,
//							Status = md.status,
//							Tagline = md.tagline,
//							IMDBRating = md.IMDBRatings,
//							RottenTomatoesRating = md.RottenTomatoesRatings,
//							MetacriticRating = md.MetacriticRatings,
//							MPA = md.rated,
//							Popularity = md.popularity
//						}).ToList();

//						await AddEntitiesInBatchesAsync(_context, movies);
//						await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
//					}
//				}
//				if (!_context.Genres.Any())
//				{
//					var genreData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\genre_details.json");
//					var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//					var genreDetails = JsonSerializer.Deserialize<List<GenreData>>(genreData, jsonOptions);

//					if (genreDetails?.Count > 0)
//					{
//						var genres = genreDetails.Select(gd => new Genre
//						{
//							Id = int.Parse(gd.id),
//							Name = gd.name
//						}).ToList();

//						await AddEntitiesInBatchesAsync(_context, genres);
//						await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
//					}
//				}
//				if (!_context.Casts.Any())
//				{
//					var castData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\cast_crew_details.json");
//					var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//					var castDetails = JsonSerializer.Deserialize<List<CastData>>(castData, jsonOptions);

//					if (castDetails?.Count > 0)
//					{
//						var casts = castDetails.Select(cd => new Cast
//						{
//							CastId = cd.id,
//							Name = cd.name,
//							Gender = cd.gender,
//							Biography = cd.biography,
//							BirthDay = !string.IsNullOrEmpty(cd.birthday) ? DateOnly.Parse(cd.birthday) : null,
//							DeathDay = !string.IsNullOrEmpty(cd.deathday) ? DateOnly.Parse(cd.deathday) : null,
//							ProfilePath = cd.profile_path,
//							PlaceOfBirth = cd.place_of_birth,
//							Popularity = cd.popularity,
//							KnownForDepartment = cd.known_for_department
//						}).ToList();

//						await AddEntitiesInBatchesAsync(_context, casts);
//						await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
//					}
//				}
//				if (!_context.CastMovie.Any())
//				{
//					// Get movie IDs for mapping
//					var movieIdMapping = await _context.Movies
//						.Select(m => new { m.TMDBId })
//						.ToDictionaryAsync(m => m.TMDBId, m => m.TMDBId);

//					// Get valid cast IDs
//					var validCastIds = await _context.Casts.Select(c => c.CastId).ToListAsync();

//					// Read the JSON data
//					var movieCastData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\movie_cast_crew_links.json");
//					var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//					var movieCasts = JsonSerializer.Deserialize<List<CastMovieDataEnhanced>>(movieCastData, jsonOptions);

//					if (movieCasts?.Count > 0)
//					{
//						var validMovieCasts = movieCasts
//							.Where(mc => int.TryParse(mc.tmdbid, out int tmdbId) && movieIdMapping.ContainsKey(tmdbId) &&
//										 int.TryParse(mc.castid, out int castId) && validCastIds.Contains(castId))
//							.GroupBy(mc => new { TmdbId = int.Parse(mc.tmdbid), CastId = int.Parse(mc.castid) }) // Group by composite key
//							.Select(g => g.First()) // Take the first entry for each unique TmdbId/CastId pair
//							.Select(mc => new CastMovie
//							{
//								TmdbId = movieIdMapping[int.Parse(mc.tmdbid)],
//								CastId = int.Parse(mc.castid),
//								Role = mc.role,
//								Extra = mc.extra
//							})
//							.ToList();

//						if (validMovieCasts.Any())
//						{
//							for (int i = 0; i < validMovieCasts.Count; i += BatchSize)
//							{
//								using var transaction = await _context.Database.BeginTransactionAsync();
//								try
//								{
//									var batch = validMovieCasts.Skip(i).Take(BatchSize).ToList();
//									await _context.CastMovie.AddRangeAsync(batch);
//									await _context.SaveChangesAsync();
//									await transaction.CommitAsync();
//								}
//								catch
//								{
//									await transaction.RollbackAsync();
//									throw;
//								}
//								_context.ChangeTracker.Clear();
//							}
//							await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
//						}
//						else
//						{
//							Console.WriteLine("No valid CastMovie entries found after filtering.");
//						}
//					}
//				}
//				if (!_context.MovieGenres.Any())
//				{
//					// Get movie IDs for mapping
//					var movieIdMapping = await _context.Movies
//						.Select(m => new { m.TMDBId })
//						.ToDictionaryAsync(m => m.TMDBId, m => m.TMDBId);

//					// Get valid genre IDs
//					var validGenreIds = await _context.Genres.Select(g => g.Id).ToListAsync();

//					// Read the JSON data
//					var movieGenreData = await File.ReadAllTextAsync("C:\\Users\\Ziad\\source\\repos\\Cinemate\\Cinemate.Repository\\Data\\DataSeed\\genre_movie_links.json");
//					var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//					var movieGenres = JsonSerializer.Deserialize<List<MovieGenreDataJson>>(movieGenreData, jsonOptions);

//					if (movieGenres?.Count > 0)
//					{
//						var validMovieGenres = movieGenres
//							.Where(mg => movieIdMapping.ContainsKey(int.Parse(mg.tmdb_id)) && validGenreIds.Contains(int.Parse(mg.genre_id)))
//							.Select(mg => new MovieGenre
//							{
//								TMDBId = movieIdMapping[int.Parse(mg.tmdb_id)],
//								GenreId = int.Parse(mg.genre_id)
//							})
//							.ToList();

//						if (validMovieGenres.Any())
//						{
//							for (int i = 0; i < validMovieGenres.Count; i += BatchSize)
//							{
//								using var transaction = await _context.Database.BeginTransactionAsync();
//								try
//								{
//									var batch = validMovieGenres.Skip(i).Take(BatchSize).ToList();
//									await _context.MovieGenres.AddRangeAsync(batch);
//									await _context.SaveChangesAsync();
//									await transaction.CommitAsync();
//								}
//								catch
//								{
//									await transaction.RollbackAsync();
//									throw;
//								}
//								_context.ChangeTracker.Clear();
//							}
//							await _context.Database.ExecuteSqlRawAsync("CHECKPOINT");
//						}
//					}
//				}
//			}
//			finally
//			{
//				_context.ChangeTracker.AutoDetectChangesEnabled = true;
//			}
//		}
//		private static async Task AddEntitiesInBatchesAsync<TEntity>(ApplicationDbContext context, List<TEntity> entities) where TEntity : class
//		{
//			for (int i = 0; i < entities.Count; i += BatchSize)
//			{
//				using var transaction = await context.Database.BeginTransactionAsync();
//				try
//				{
//					var batch = entities.Skip(i).Take(BatchSize).ToList();
//					await context.Set<TEntity>().AddRangeAsync(batch);
//					await context.SaveChangesAsync();
//					await transaction.CommitAsync();
//				}
//				catch
//				{
//					await transaction.RollbackAsync();
//					throw;
//				}
//				context.ChangeTracker.Clear();
//			}
//		}
//		private class CastMovieDataEnhanced
//		{
//			public string tmdbid { get; set; } = string.Empty;
//			public string castid { get; set; } = string.Empty;
//			public string? role { get; set; }
//			public string? extra { get; set; }
//		}
//		private class MovieGenreDataJson
//		{
//			public string tmdb_id { get; set; } = string.Empty;
//			public string genre_id { get; set; } = string.Empty;
//		}
//		private class MovieData
//		{
//			// Change these from integers to strings
//			public string tmdb_id { get; set; } = string.Empty;
//			public int imdb_id { get; set; }
//			public string? title { get; set; }
//			public string? overview { get; set; }
//			public string? release_date { get; set; }
//			public int? runtime { get; set; }
//			public string? language { get; set; }
//			public string? poster { get; set; }
//			public string? backdrop { get; set; }
//			public string? trailer { get; set; }
//			public double? budget { get; set; }
//			public double? revenue { get; set; }
//			public string? status { get; set; }
//			public string? tagline { get; set; }
//			[JsonPropertyName("IMDB ratings")]
//			public string? IMDBRatings { get; set; }
//			[JsonPropertyName("Rotten Tomatoes ratings")]
//			public string? RottenTomatoesRatings { get; set; }
//			[JsonPropertyName("Metacritic ratings")]
//			public string? MetacriticRatings { get; set; }
//			public string? rated { get; set; }
//			public double? popularity { get; set; }
//		}
//		private class GenreData
//		{
//			public string id { get; set; } = string.Empty;
//			public string name { get; set; } = string.Empty;
//		}
//		private class CastData
//		{
//			public int id { get; set; }
//			public string? name { get; set; }
//			public int? gender { get; set; }
//			public string? biography { get; set; }
//			public string? birthday { get; set; }
//			public string? deathday { get; set; }
//			public string? profile_path { get; set; }
//			public string? place_of_birth { get; set; }
//			public double? popularity { get; set; }
//			public string? known_for_department { get; set; }
//			public string? imdb_id { get; set; }
//			public bool adult { get; set; }
//		}
//	}
//}