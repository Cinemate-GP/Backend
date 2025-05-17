using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cinemate.Repository.Abstractions;
using static Cinemate.Repository.Errors.Authentication.AuthenticationError;
using Microsoft.Extensions.DependencyInjection;


namespace Cinemate.Service.Services.Follow_Service
{
    public class UserFollowService : IUserfollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;
         public UserFollowService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
         {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
		 }
        public async Task<OperationResult> AddUserFollowAsync(AddFollowRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate input
                if (request == null || string.IsNullOrEmpty(request.FollowId))
                    return OperationResult.Failure("Invalid follow request.");

                // Get user ID from claims
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return OperationResult.Failure("Unauthorized user.");

                // Prevent self-following
                if (userId == request.FollowId)
                    return OperationResult.Failure("Cannot follow yourself.");

                var existingFollow = await _unitOfWork.Repository<UserFollow>()
                      .GetQueryable()
                      .Where(uf => uf.UserId == userId && uf.FollowId == request.FollowId)
                      .FirstOrDefaultAsync(cancellationToken);
                if (existingFollow != null)
                    return OperationResult.Failure("You are already following this user.");
               

                // Create new follow entity
                var entity = new UserFollow
                {
                    UserId = userId,
                    FollowId = request.FollowId,
                    FollowedOn = DateTime.UtcNow 
                };

                // Add and save
                await _unitOfWork.Repository<UserFollow>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success("User followed successfully.");
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Failure("Failed to follow user due to database error.");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Failed to follow user.");
            }
        }
        public async Task<OperationResult> DeleteUserFollowAsync(AddFollowRequest request, CancellationToken cancellationToken = default)
        {
            try
            {

                if (request == null || string.IsNullOrEmpty(request.FollowId))
                    return OperationResult.Failure("Invalid unfollow request.");

                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return OperationResult.Failure("Unauthorized user.");

                var followEntity = await _unitOfWork.Repository<UserFollow>()
                    .GetQueryable()
                    .Where(uf => uf.UserId == userId && uf.FollowId == request.FollowId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (followEntity == null)
                    return OperationResult.Failure("Follow relationship not found.");

                _unitOfWork.Repository<UserFollow>().Delete(followEntity);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success("Unfollowed successfully.");
            }
            catch (Exception)
            {
                return OperationResult.Failure("Failed to unfollow user.");
            }
        }
        public async Task<IEnumerable<UserDataFollow>> GetAllFollowers(string userName, CancellationToken cancellationToken = default)
        {
			var userIdToken = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFollowRepo = _unitOfWork.Repository<UserFollow>().GetQueryable();
            var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
			var user = await userRepo.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
			if (user == null)
				return Enumerable.Empty<UserDataFollow>();

            var followers = await (from follow in userFollowRepo
								   join followerUser in userRepo on follow.UserId equals followerUser.Id
								   where follow.FollowId == user.Id
                                   select new UserDataFollow
                                   {
									   UserId = followerUser.Id,
									   FullName = followerUser.FullName,
									   ProfilePic = followerUser.ProfilePic,
                                       followedOn = follow.FollowedOn,
									   IsFollow = userFollowRepo.Any(uf => uf.UserId == userIdToken && uf.FollowId == followerUser.Id)
                                   }).ToListAsync(cancellationToken);

            return followers;
        }
		public async Task<IEnumerable<UserDataFollow>> GetAllFollowing(string userName, CancellationToken cancellationToken = default)
        {
			var userIdToken = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFollowRepo = _unitOfWork.Repository<UserFollow>().GetQueryable();
            var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
			var user = await userRepo.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
			if (user == null)
				return Enumerable.Empty<UserDataFollow>();

            var following = await (from follow in userFollowRepo
                                   join followedUser in userRepo on follow.FollowId equals user.Id
                                   where follow.UserId == user.Id
                                   select new UserDataFollow
                                   {
                                       UserId = user.Id,
                                       FullName = user.FullName,
                                       ProfilePic = user.ProfilePic,
                                       followedOn = follow.FollowedOn,
									   IsFollow = userFollowRepo.Any(uf => uf.UserId == userIdToken && uf.FollowId == user.Id)
                                   }).ToListAsync(cancellationToken);

            return following;
        }

		public async Task<Result<FollowerDetailsResponse>> GetFollowersDetailsAsync(string userId, string followName, CancellationToken cancellationToken = default)
        {
			var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
			var user = await userRepo.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
			if (user is null)
				return Result.Failure<FollowerDetailsResponse>(UserErrors.UserNotFound);

			var follower = await userRepo.FirstOrDefaultAsync(u => u.UserName == followName, cancellationToken);
			if (follower is null)
				return Result.Failure<FollowerDetailsResponse>(UserErrors.FollowerNotFound);

			var userFollowRepo = _unitOfWork.Repository<UserFollow>().GetQueryable();
			var isFollowing = await userFollowRepo.AnyAsync(uf => uf.UserId == userId && uf.FollowId == follower.Id, cancellationToken);

			var profileService = _serviceProvider.GetRequiredService<IProfileService>();
			var recentActivity = await profileService.GetAllRecentActivity(follower.Id, cancellationToken);

			var response = new FollowerDetailsResponse(
				follower.Id,
				follower.FullName,
				follower.ProfilePic,
				isFollowing,
				recentActivity.Value
	        );
			return Result.Success(response);
		}
	}
}
