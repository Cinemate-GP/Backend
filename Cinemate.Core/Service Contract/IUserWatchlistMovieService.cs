using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Errors.ProfileError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
    public interface IUserWatchlistMovieService
    {
        Task<OperationResult> AddUserWatchlistMovieAsync(UserWatchListMovieResponse userWatchlistMovieResponse, CancellationToken cancellationToken = default);

        Task<OperationResult> DeleteUserWatchlistMovieAsync(UserWatchListMovieResponse response, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserWatchListMovieResponseBack>> GetUserWatchlistMoviesAsync(CancellationToken cancellationToken = default);





    }
}
