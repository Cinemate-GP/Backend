using Cinemate.Core.Contracts.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
	public interface IMovieService
	{
		Task<IEnumerable<MoviesTopTenResponse>> GetMovieTopTenAsync(CancellationToken cancellationToken = default);
		Task<Result<MovieDetailsResponse>> GetMovieDetailsAsync(int tmdbid, CancellationToken cancellationToken = default);
		Task<IEnumerable<MovieDetailsRandomResponse>> GetMovieRandomAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<MoviesTopTenResponse>> GetMovieBasedOnGeneraAsync(MovieGeneraRequest? request, CancellationToken cancellationToken = default);
	}
}
