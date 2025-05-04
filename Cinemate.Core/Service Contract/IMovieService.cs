using Cinemate.Core.Abstractions;
using Cinemate.Core.Contracts.Common;
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
		Task<IEnumerable<MoviesTopTenResponse>> GetMovieTopTenRatedAsync(CancellationToken cancellationToken = default);
		Task<Result<MovieDetailsResponse>> GetMovieDetailsAsync(string userId, int tmdbid, CancellationToken cancellationToken = default);
		Task<IEnumerable<MovieDetailsRandomResponse>> GetMovieRandomAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<MovieTrendingResponse>> GetTrendingMoviesAsync(CancellationToken cancellationToken = default);
		Task<IEnumerable<MoviesTopTenResponse>> GetMovieBasedOnGeneraAsync(MovieGeneraRequest? request, CancellationToken cancellationToken = default);
		Task<PaginatedList<MoviesTopTenResponse>> GetPaginatedMovieBasedAsync(RequestFilters request, CancellationToken cancellationToken = default);
		Task<Result<IEnumerable<SearchResponse>>> GetSearchForMovieActorUsersAsync(RequestSearch request, CancellationToken cancellationToken = default);
		Task<int> FetchAndSaveUpcomingMoviesAsync(CancellationToken cancellationToken = default);
		Task<int> UpdateMovieRatingsAsync(CancellationToken cancellationToken = default);
		Task<int> FetchAndSaveMovieByTmdbIdAsync(int tmdbId, CancellationToken cancellationToken = default);
	}
}
