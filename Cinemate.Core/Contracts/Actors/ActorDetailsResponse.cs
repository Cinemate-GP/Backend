using Cinemate.Core.Contracts.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Actors
{
    public record ActorDetailsResponse
    (
        int Id,
		string Name,
		string? ProfilePath,
		IEnumerable<MoviesTopTenResponse> Movies
	);
}
