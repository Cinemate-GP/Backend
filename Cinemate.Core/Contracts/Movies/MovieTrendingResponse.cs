using Cinemate.Core.Contracts.Genres;
using System;
using System.Collections.Generic;

namespace Cinemate.Core.Contracts.Movies
{
    public record MovieTrendingResponse
	(
		int TMDBId,
		string? Title,
		string? LogoPath,
		string? Tagline,
		string? PosterPath,
        string? BackdropPath,
		string? IMDBRating,
        int? Runtime,
		DateOnly? ReleaseDate,
		string? Language,
		string? Trailer,
		IEnumerable<GenresDetails> GenresDetails
	);
}