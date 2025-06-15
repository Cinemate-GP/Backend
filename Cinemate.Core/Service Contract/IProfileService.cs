using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Contracts.Movies;
using Cinemate.Core.Contracts.Profile;
using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Recent_Activity;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Repository.Abstractions;

namespace Cinemate.Core.Service_Contract
{
    public interface IProfileService
    {
        Task<Result<UpdateProfileReauestBack>> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default);
		Task<OperationResult> DeleteAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserWatchedMovieResponseBack>> GetAllMoviesWatched(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserLikeMovieResponseBack>> GetAllMoviesLiked(CancellationToken cancellationToken = default);
        Task< IEnumerable<UserRateMovieResponseBack>> GetAllMoviesRated(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserReviewMovieResponseBack>> GetAllMoviesReviews(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserWatchListMovieResponseBack>> GetAllWatchlist(CancellationToken cancellationToken = default);
		Task<Result<GetUserDetailsResponse>> GetUserDetailsAsync(string userName, CancellationToken cancellationToken = default);
		Task<Result<IEnumerable<UserRecentActivityResponse>>> GetAllRecentActivity(string userName, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<FeedResponse>>> GetFeedForUserAsync(string id, CancellationToken cancellationToken = default);
        Task<Result> ToggleFollowerAndFollowing(string userName, CancellationToken cancellationToken = default);
        Task<Result> ToggleRecentActivity(string userName, CancellationToken cancellationToken = default);
        Task<Result> ToggleNotificationFollowing(string userName, CancellationToken cancellationToken = default);
        Task<Result> ToggleNotificationNewRelease(string userName, CancellationToken cancellationToken = default);
        Task<Result<PrivacyResponse>> GetPrivacyAsync(string userName, CancellationToken cancellationToken = default);
        Task<Result<NotificationPrivacyResponse>> GetNotificationPrivacy(string userName, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<MoviesTopTenResponse>>> CalculateUserTestAsync(string userName, CancellationToken cancellationToken = default);
        Task<Result> TestMLRecommendationFlowAsync(string userName, List<MovieRatingItem> ratings, CancellationToken cancellationToken = default);
	}
}
