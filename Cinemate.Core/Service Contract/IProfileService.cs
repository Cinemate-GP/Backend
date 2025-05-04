using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Contracts.Profile;
using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Recent_Activity;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
    public interface IProfileService
    {
        Task<UpdateProfileReauestBack> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default);
        Task<OperationResult> DeleteAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserWatchedMovieResponseBack>> GetAllMoviesWatched(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserLikeMovieResponseBack>> GetAllMoviesLiked(CancellationToken cancellationToken = default);
        Task< IEnumerable<UserRateMovieResponseBack>> GetAllMoviesRated(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserReviewMovieResponseBack>> GetAllMoviesReviews(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserWatchListMovieResponseBack>> GetAllWatchlist(CancellationToken cancellationToken = default);
		Task<Result<GetUserDetailsResponse>> GetUserDetailsAsync(string userId, CancellationToken cancellationToken = default);
		Task<Result<IEnumerable<UserRecentActivityResponse>>> GetAllRecentActivity(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDataFollow>> GetAllFollowers(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDataFollow>> GetAllFollowing(CancellationToken cancellationToken = default);
        Task<int> CountFollowers(CancellationToken cancellationToken = default);
        Task<int> CountFollowing(CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<FeedResponse>>> GetFeedForUserAsync(string id, CancellationToken cancellationToken = default);
	}
}
