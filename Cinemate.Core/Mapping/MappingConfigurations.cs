using Cinemate.Core.Contracts.Actors;
using Cinemate.Core.Contracts.Genres;
using Cinemate.Core.Contracts.Movies;
using Cinemate.Core.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				.Map(dest => dest.MovieReviews, src => new List<MovieReviewResponse>())
				.Map(dest => dest.IsLiked, src => false)
				.Map(dest => dest.IsInWatchList, src => false)
				.Map(dest => dest.IsWatched, src => false)
				.Map(dest => dest.Stars, src => (int?)null);

			config.NewConfig<Movie, MoviesTopTenResponse>()
				.Map(dest => dest.TMDBId, src => src.TMDBId)
				.Map(dest => dest.IMDBRating, src => string.IsNullOrEmpty(src.IMDBRating) ? "0.0/10" : src.IMDBRating)
				.Map(dest => dest.MPA, src => string.IsNullOrEmpty(src.MPA) ? "Not Rated" : src.MPA);

			config.NewConfig<Movie, MovieTrendingResponse>()
				.Map(dest => dest.GenresDetails, src =>
					src.MovieGenres.Select(mg => new GenresDetails(mg.Genre.Id, mg.Genre.Name ?? string.Empty)))
				.Map(dest => dest.IMDBRating, src => src.IMDBRating == null ? "0.0/10" : src.IMDBRating);

			config.NewConfig<Movie, MovieDetailsRandomResponse>()
				.Map(dest => dest.GenresDetails, src =>
					src.MovieGenres.Select(mg => new GenresDetails(mg.Genre.Id, mg.Genre.Name ?? string.Empty)))
				.Map(dest => dest.IMDBRating, src => src.IMDBRating == null ? "0.0/10" : src.IMDBRating)
				.Map(dest => dest.ReleasDate, src => src.ReleaseDate);

			config.NewConfig<Cast, ActorDetailsResponse>();

		}
    }
}
