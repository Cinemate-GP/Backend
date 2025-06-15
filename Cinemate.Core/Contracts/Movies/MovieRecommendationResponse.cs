using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Movies
{
	public record MovieRecommendationResponse
	(
		int TMDBId,
		string? Title,
		string? PosterPath,
		string? IMDBRating,
		string? MPA,
		double Score
	);
}
