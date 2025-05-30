﻿using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Errors.ProfileError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
    public interface IUserfollowService
    {
        Task<OperationResult> AddUserFollowAsync(AddFollowRequest response, CancellationToken cancellationToken = default);
        Task<OperationResult> DeleteUserFollowAsync(AddFollowRequest response, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDataFollow>> GetAllFollowers(string userName, CancellationToken cancellationToken = default);
		Task<IEnumerable<UserDataFollow>> GetAllFollowing(string userName, CancellationToken cancellationToken = default);
        Task<Result<FollowerDetailsResponse>> GetFollowersDetailsAsync(string userId, string followName, CancellationToken cancellationToken = default);
	}
}
