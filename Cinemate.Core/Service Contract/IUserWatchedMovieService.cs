using Cinemate.Core.Contracts.User_Like;
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
    public interface IUserWatchedMovieService
    {
        Task<OperationResult> AddUserWatchedMovieAsync(UserWatchedMovieResponse userWatchedMovieResponse, CancellationToken cancellationToken = default);

        Task<OperationResult> DeleteUserWatchedMovieAsync(UserWatchedMovieResponse response, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserWatchedMovie>> GetUserWatchedMoviesAsync(CancellationToken cancellationToken = default);





    }
}
