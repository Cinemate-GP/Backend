using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Errors.ProfileError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
    public interface IUserRateMovieService
    {
        Task<OperationResult> AddUserRateMovieAsync(UserRateMovieResponse userRateMovieResponse, CancellationToken cancellationToken = default);
        Task<OperationResult> DeleteUserRateMovieAsync(UserRateMovieResponse response, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserRateMovieResponseBack>> GetUserRateMoviesAsync(CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserRateMovieResponseBack>>> GetMoviesRatedByUserAsync(string userId, CancellationToken cancellationToken = default);
	}
}
