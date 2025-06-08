using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Repository.Abstractions;

namespace Cinemate.Core.Service_Contract
{
    public interface IUserfollowService
    {
        Task<OperationResult> AddUserFollowAsync(AddFollowRequest response, CancellationToken cancellationToken = default);
        Task<OperationResult> DeleteUserFollowAsync(AddFollowRequest response, CancellationToken cancellationToken = default);
		Task<Result<IEnumerable<UserDataFollow>>> GetAllFollowers(string userName, CancellationToken cancellationToken = default);
		Task<Result<IEnumerable<UserDataFollow>>> GetAllFollowing(string userName, CancellationToken cancellationToken = default);
		Task<Result<FollowerDetailsResponse>> GetFollowersDetailsAsync(string userId, string followName, CancellationToken cancellationToken = default);
		Task<Result> RemoveFollowersAsync(string userName, RemoveFollowerRequest request, CancellationToken cancellationToken = default);
	}
}
