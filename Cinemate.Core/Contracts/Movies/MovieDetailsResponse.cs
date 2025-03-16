using Cinemate.Core.Contracts.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Movies
{
	public record MovieDetailsResponse
	(
		int MovieId,
		int TMDBId,
		string? title,
		string? Overview,
		string? Poster_path,
		int? Runtime,
		DateOnly? Release_date,
		string? Trailer_path,
		IEnumerable<ActorMovieResponse> Actors
	);
}
