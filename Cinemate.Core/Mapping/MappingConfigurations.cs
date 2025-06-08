using Cinemate.Core.Contracts.Actors;
using Cinemate.Core.Contracts.Genres;
using Cinemate.Core.Contracts.Movies;
using Cinemate.Core.Entities;
using Mapster;

namespace Cinemate.Core.Mapping
{
	public class MappingConfigurations : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Movie, MovieDetailsResponse>()
				.Map(dest => dest.GenresDetails, src =>
					src.MovieGenres.Select(mg => new GenresDetails(mg.Genre.Id, mg.Genre.Name ?? string.Empty)))
				.Map(dest => dest.Actors, src =>
					src.CastMovies.Select(cm => new ActorMovieResponse(
						cm.Cast.CastId,
						cm.Cast.Name ?? string.Empty,
						cm.Cast.ProfilePath,
						cm.Role,
						cm.Extra)))
				.Map(dest => dest.TMDBId, src => src.TMDBId)
				.Map(dest => dest.IMDBRating, src => string.IsNullOrEmpty(src.IMDBRating) ? "0.0/10" : src.IMDBRating)
				.Map(dest => dest.RottenTomatoesRating, src => string.IsNullOrEmpty(src.RottenTomatoesRating) ? null : src.RottenTomatoesRating)
				.Map(dest => dest.MetacriticRating, src => string.IsNullOrEmpty(src.MetacriticRating) ? null : src.MetacriticRating)
				.Map(dest => dest.MPA, src => string.IsNullOrEmpty(src.MPA) ? "Not Rated" : src.MPA)
				.Map(dest => dest.MovieReviews, src => new List<MovieReviewResponse>());

			config.NewConfig<Movie, MoviesTopTenResponse>()
				.Map(dest => dest.TMDBId, src => src.TMDBId)
				.Map(dest => dest.IMDBRating, src => string.IsNullOrEmpty(src.IMDBRating) ? "0.0/10" : src.IMDBRating)
				.Map(dest => dest.MPA, src => string.IsNullOrEmpty(src.MPA) ? "Not Rated" : src.MPA);

			config.NewConfig<Movie, MovieTrendingResponse>()
				.Map(dest => dest.GenresDetails, src =>
					src.MovieGenres.Select(mg => new GenresDetails(mg.Genre.Id, mg.Genre.Name ?? string.Empty)))
				.Map(dest => dest.IMDBRating, src => src.IMDBRating == null ? "0.0/10" : src.IMDBRating)
				.Map(dest => dest.TMDBId, src => src.TMDBId);

			config.NewConfig<Movie, MovieDetailsRandomResponse>()
				.Map(dest => dest.GenresDetails, src =>
					src.MovieGenres.Select(mg => new GenresDetails(mg.Genre.Id, mg.Genre.Name ?? string.Empty)))
				.Map(dest => dest.IMDBRating, src => src.IMDBRating == null ? "0.0/10" : src.IMDBRating)
				.Map(dest => dest.ReleasDate, src => src.ReleaseDate)
				.Map(dest => dest.TMDBId, src => src.TMDBId);

			config.NewConfig<Cast, ActorDetailsResponse>()
				.Map(dest => dest.Id, src => src.CastId)
				.Map(dest => dest.Name, src => src.Name)
				.Map(dest => dest.Biography, src => src.Biography)
				.Map(dest => dest.BirthDay, src => src.BirthDay)
				.Map(dest => dest.DeathDay, src => src.DeathDay)
				.Map(dest => dest.ProfilePath, src => src.ProfilePath)
				.Map(dest => dest.PlaceOfBirth, src => src.PlaceOfBirth)
				.Map(dest => dest.Popularity, src => src.Popularity)
				.Map(dest => dest.KnownForDepartment, src => src.KnownForDepartment)
				.Map(dest => dest.Movies, src => new List<MoviesTopTenResponse>());

			config.NewConfig<Cast, ActorDetailsResponse>();

		}
    }
}
