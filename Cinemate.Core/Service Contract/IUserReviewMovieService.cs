using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Errors.ProfileError;

namespace Cinemate.Core.Service_Contract
{
    public interface IUserReviewMovieService
    {
        Task<OperationResult> AddUserReviewMovieAsync(UserReviewMovieResponse userReviewMovieResponse, CancellationToken cancellationToken = default);
        Task<OperationResult> DeleteUserReviewMovieAsync(UserReviewDeleteMovieResponse userReviewDeleteMovieResponse, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserReviewMovieResponseBack>> GetUserReviewMoviesAsync(CancellationToken cancellationToken = default);
    }
}
