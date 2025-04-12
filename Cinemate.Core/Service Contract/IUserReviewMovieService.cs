using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Errors.ProfileError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
    public interface IUserReviewMovieService
    {
        Task<OperationResult> AddUserReviewMovieAsync(UserReviewMovieResponse userReviewMovieResponse, CancellationToken cancellationToken = default);

        Task<OperationResult> DeleteUserReviewMovieAsync(UserReviewDeleteMovieResponse userReviewDeleteMovieResponse, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserReviewMovie>> GetUserReviewMoviesAsync(CancellationToken cancellationToken = default);








    }
}
