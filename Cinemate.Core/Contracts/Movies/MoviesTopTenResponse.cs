using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Movies
{
    public record MoviesTopTenResponse
    (
		int MovieId,
		int TMDBId,
		string? Title,
		string? Poster_path
	);
}
