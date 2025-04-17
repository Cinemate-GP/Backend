using Cinemate.Core.Contracts.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemate.Core.Contracts.Genres;

namespace Cinemate.Core.Contracts.Movies
{
    public record MovieDetailsRandomResponse
	(
		int TMDBId,
		string? Title,
		string? Tagline,
		string? PosterPath,
        string? BackdropPath,
		string? IMDBRating,
        int? Runtime,
		DateOnly? ReleasDate,
		string? Trailer,
		IEnumerable<GenresDetails> GenresDetails
	);
}
