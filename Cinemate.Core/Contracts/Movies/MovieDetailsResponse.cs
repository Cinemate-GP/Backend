using Cinemate.Core.Contracts.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemate.Core.Contracts.Genres;

namespace Cinemate.Core.Contracts.Movies
{
	public record MovieDetailsResponse
	(
		int TMDBId,
		string? Title,
		string? Overview,
        string? Tagline,
        string? PosterPath,
        string? BackdropPath,
        string? Language,
        int? Runtime,
		DateOnly? ReleaseDate,
		string? Trailer,
        string? IMDBRating,
        string? RottenTomatoesRating,
        string? MetacriticRating,
        string? MPA,
		int? Stars,
		bool IsLiked,
		bool IsInWatchList,
		bool IsWatched,
		IEnumerable<ActorMovieResponse> Actors,
		IEnumerable<GenresDetails> GenresDetails,
		IEnumerable<MovieReviewResponse> MovieReviews
	);
}
