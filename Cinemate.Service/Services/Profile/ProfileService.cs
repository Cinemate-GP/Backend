﻿using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Contracts.Movies;
using Cinemate.Core.Contracts.Profile;
using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Recent_Activity;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.Movie;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Cinemate.Repository.Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Cinemate.Repository.Errors.Authentication.AuthenticationError;
using IFileService = Cinemate.Core.Service_Contract.IFileService;
using Result = Cinemate.Repository.Abstractions.Result;

namespace Cinemate.Service.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserLikeMovieService _userLikeMovieService;
        private readonly IUserRateMovieService _userRateMovieService;
        private readonly IUserReviewMovieService _userReviewMovieService;
        private readonly IUserWatchedMovieService _userWatchedMovieService;
        private readonly IUserWatchlistMovieService _userWatchlistService;
		private readonly ApplicationDbContext _context;
		private readonly IUnitOfWork _unitOfWork;
		private readonly HttpClient _httpClient;
		private const string MLMovieRecommenderUrl = "http://cinemate-rs-api.ebe5hqc4gueefzed.italynorth.azurecontainer.io:5000/api/v1/user/test";

		public ProfileService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IFileService fileService, IHttpContextAccessor httpContextAccessor, IUserLikeMovieService userLikeMovieService, IUserRateMovieService userRateMovieService, IUserReviewMovieService userReviewMovieService, IUserWatchedMovieService userWatchedMovieService, IUserWatchlistMovieService userWatchlistService, ApplicationDbContext context, IUnitOfWork unitOfWork, HttpClient httpClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
            _userLikeMovieService = userLikeMovieService;
            _userRateMovieService = userRateMovieService;
            _userReviewMovieService = userReviewMovieService;
            _userWatchedMovieService = userWatchedMovieService;
            _userWatchlistService = userWatchlistService;            
            _context = context;
			_unitOfWork = unitOfWork;
			_httpClient = httpClient;
		}		
		public async Task<OperationResult> DeleteAsync(string userName, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
				return OperationResult.Failure("User not found.");

			var followsAsFollower = await _context.UserFollows
				.Where(uf => uf.UserId == user.Id)
				.ToListAsync(cancellationToken);

			var followsAsFollowed = await _context.UserFollows
				.Where(uf => uf.FollowId == user.Id)
				.ToListAsync(cancellationToken);

			_context.UserFollows.RemoveRange(followsAsFollower);
			_context.UserFollows.RemoveRange(followsAsFollowed);
			await _context.SaveChangesAsync(cancellationToken);
			var result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
				return OperationResult.Failure("Failed to delete user account.");

			await _signInManager.SignOutAsync();
			return OperationResult.Success("User deleted and signed out.");
		}
		public async Task<IEnumerable<UserLikeMovieResponseBack>> GetAllMoviesLiked(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserLikeMovieResponseBack>();
            var result= await _userLikeMovieService.GetUserLikeMoviesAsync(cancellationToken);
           
            return result.Where(r => r.UserId == userId);

        }
        public async Task<IEnumerable<UserRateMovieResponseBack>> GetAllMoviesRated(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserRateMovieResponseBack>();
            var result = await _userRateMovieService.GetUserRateMoviesAsync(cancellationToken);

            return result.Where(r => r.UserId == userId);
        }
        public async Task<IEnumerable<UserReviewMovieResponseBack>> GetAllMoviesReviews(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserReviewMovieResponseBack>();

            var result = await _userReviewMovieService.GetUserReviewMoviesAsync(cancellationToken);

            return result.Where(r => r.UserId == userId);
        }
        public async Task<IEnumerable<UserWatchedMovieResponseBack>> GetAllMoviesWatched(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserWatchedMovieResponseBack>();
            var result = await _userWatchedMovieService.GetUserWatchedMoviesAsync(cancellationToken);

            return result.Where(r => r.UserId == userId);
        }
        public async Task<IEnumerable<UserWatchListMovieResponseBack>> GetAllWatchlist(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserWatchListMovieResponseBack>();
            var result = await _userWatchlistService.GetUserWatchlistMoviesAsync(cancellationToken);

            return result.Where(r => r.UserId == userId);
        }
        public async Task<Result<UpdateProfileReauestBack>> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
				return Result.Failure<UpdateProfileReauestBack>(UserErrors.Unauthorized);

			var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
				return Result.Failure<UpdateProfileReauestBack>(UserErrors.DublicatedUserName);

			if (request.FullName != null)
                user.FullName = request.FullName;

			if (!string.IsNullOrWhiteSpace(request.UserName) && request.UserName != user.UserName)
			{
				var userNameIsExist = await _userManager.Users.AnyAsync(x => x.UserName == request.UserName, cancellationToken);
				if (userNameIsExist)
					return Result.Failure<UpdateProfileReauestBack>(UserErrors.DublicatedUserName);

				var userNameResult = await _userManager.SetUserNameAsync(user, request.UserName);
				if (!userNameResult.Succeeded)
				{
					var error = userNameResult.Errors.First();
					return Result.Failure<UpdateProfileReauestBack>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
				}
			}
			if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
				if (request.Email.Contains("@gmail.com") || request.Email.Contains("@yahoo.com") || request.Email.Contains("@fayoum.edu.eg"))
				{
					var emailResult = await _userManager.SetEmailAsync(user, request.Email);
					if (!emailResult.Succeeded)
					{
						var error = emailResult.Errors.First();
						return Result.Failure<UpdateProfileReauestBack>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
					}
				}
				else
					return Result.Failure<UpdateProfileReauestBack>(UserErrors.InvalidEmailDomain);
			}
			if (!string.IsNullOrWhiteSpace(request.Bio))
				user.Bio = request.Bio;

			if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (!passwordResult.Succeeded)
                {
					var error = passwordResult.Errors.First();
					return Result.Failure<UpdateProfileReauestBack>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
				}
            }
            if (request.Profile_Image != null && request.Profile_Image.Length > 0)
            {
                // Delete old profile image if it exists
                if (!string.IsNullOrEmpty(user.ProfilePic))
                {
                    var oldFileName = Path.GetFileName(user.ProfilePic);
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Profile_Image", oldFileName);
                    if (System.IO.File.Exists(oldFilePath))
                        _fileService.DeleteFile(oldFileName, ImageSubFolder.Profile_Image);
                }

                string createdImageName = await _fileService.SaveFileAsync(request.Profile_Image, ImageSubFolder.Profile_Image);
                string baseUrl = GetBaseUrl("Profile_Image");
                user.ProfilePic = $"{baseUrl}{createdImageName}";
            }
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
				var error = updateResult.Errors.First();
				return Result.Failure<UpdateProfileReauestBack>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
			}
			var Updated = new UpdateProfileReauestBack
            {
				UserName = user.UserName,
				Email = user.Email,
				Bio = user.Bio,
				Profile_Image = user.ProfilePic,
                FullName = user.FullName,
            };
			return Result.Success(Updated);
        }
		public async Task<Result<IEnumerable<FeedResponse>>> GetFeedForUserAsync(string id, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
				return Result.Failure<IEnumerable<FeedResponse>>(UserErrors.UserNotFound);

			var userFollows = await _context.UserFollows
				.Where(f => f.UserId == id)
				.Select(f => new { f.FollowId, f.FollowedOn })
				.ToListAsync(cancellationToken);

			if (!userFollows.Any())
				return Result.Success(Enumerable.Empty<FeedResponse>());

			var followedUserIds = userFollows.Select(f => f.FollowId).ToList();

			var privacySettings = await _context.Users
				.Where(u => followedUserIds.Contains(u.Id))
				.Select(u => new { u.Id, u.IsEnableRecentActivity, u.IsEnableFollowerAndFollowing })
				.ToListAsync(cancellationToken);

			var userIdsWithHiddenActivity = privacySettings
				.Where(p => p.IsEnableRecentActivity)
				.Select(p => p.Id)
				.ToList();

			var userIdsWithHiddenFollows = privacySettings
				.Where(p => p.IsEnableFollowerAndFollowing)
				.Select(p => p.Id)
				.ToList();

			// Like Activities
			var likeActivities = await _context.UserLikeMovies
				.Where(l => followedUserIds.Contains(l.UserId) && !userIdsWithHiddenActivity.Contains(l.UserId))
				.Select(l => new
				{
					l.UserId,
					l.User.UserName,
					UserFullName = l.User.FullName,
					UserProfilePic = l.User.ProfilePic,
					l.TMDBId,
					MoviePosterPath = l.Movie.PosterPath,
					MovieBackdropPath = l.Movie.BackdropPath,
					MovieTitle = l.Movie.Title,
					l.LikedOn
				})
				.ToListAsync(cancellationToken);

			likeActivities = likeActivities
				.Join(userFollows,
					like => like.UserId,
					follow => follow.FollowId,
					(like, follow) => new { Like = like, FollowedOn = follow.FollowedOn })
				.Where(x => x.Like.LikedOn > x.FollowedOn)
				.Select(x => x.Like)
				.ToList();

			var likeResponses = likeActivities
				.OrderByDescending(l => l.LikedOn)
				.Select(l => new FeedResponse(
					l.UserName!,
					l.UserFullName,
					l.UserProfilePic,
					"like",
					l.TMDBId.ToString(),
					l.MoviePosterPath,
					l.MovieBackdropPath, 
					l.MovieTitle,
					$"liked {l.MovieTitle}",
					l.LikedOn
				)).ToList();

			// Follow Activities
			var followActivities = await _context.UserFollows
				.Where(f => followedUserIds.Contains(f.UserId) && !userIdsWithHiddenFollows.Contains(f.UserId))
				.Select(f => new
				{
					f.UserId,
					FollowerUserName = f.Follower.UserName,
					FollowerFullName = f.Follower.FullName,
					FollowerProfilePic = f.Follower.ProfilePic,
					f.FollowId,
					FollowedUserName = f.FollowedUser.UserName,
					FollowedUserFullName = f.FollowedUser.FullName,
					FollowedUserProfilePic = f.FollowedUser.ProfilePic,
					f.FollowedOn
				})
				.ToListAsync(cancellationToken);

			followActivities = followActivities
				.Join(userFollows,
					follow => follow.UserId,
					userFollow => userFollow.FollowId,
					(follow, userFollow) => new { Follow = follow, FollowedOn = userFollow.FollowedOn })
				.Where(x => x.Follow.FollowedOn > x.FollowedOn)
				.Select(x => x.Follow)
				.ToList();

			var followResponses = followActivities
				.OrderByDescending(f => f.FollowedOn)
				.Select(f => new FeedResponse(
					f.FollowerUserName!,
					f.FollowerFullName,
					f.FollowerProfilePic,
					"follow",
					f.FollowedUserName!,
					f.FollowedUserProfilePic,
					null,
					f.FollowedUserFullName,
					$"followed {f.FollowedUserFullName}",
					f.FollowedOn
				)).ToList();

			// Review Activities
			var reviewActivities = await _context.UserReviewMovies
				.Where(r => followedUserIds.Contains(r.UserId) && !userIdsWithHiddenActivity.Contains(r.UserId))
				.Select(r => new
				{
					r.UserId,
					r.User.UserName,
					UserFullName = r.User.FullName,
					UserProfilePic = r.User.ProfilePic,
					r.TMDBId,
					MoviePosterPath = r.Movie.PosterPath,
					MovieBackdropPath = r.Movie.BackdropPath, 
					MovieTitle = r.Movie.Title,
					r.ReviewBody,
					r.ReviewedOn
				})
				.ToListAsync(cancellationToken);

			reviewActivities = reviewActivities
				.Join(userFollows,
					review => review.UserId,
					follow => follow.FollowId,
					(review, follow) => new { Review = review, FollowedOn = follow.FollowedOn })
				.Where(x => x.Review.ReviewedOn > x.FollowedOn)
				.Select(x => x.Review)
				.ToList();

			var reviewResponses = reviewActivities
				.OrderByDescending(r => r.ReviewedOn)
				.Select(r => new FeedResponse(
					r.UserName,
					r.UserFullName,
					r.UserProfilePic,
					"review",
					r.TMDBId.ToString(),
					r.MoviePosterPath,
					r.MovieBackdropPath,
					r.MovieTitle,
					r.ReviewBody,
					r.ReviewedOn
				)).ToList();

			// Rate Activities
			var rateActivities = await _context.UserRateMovies
				.Where(r => followedUserIds.Contains(r.UserId) && !userIdsWithHiddenActivity.Contains(r.UserId))
				.Select(r => new
				{
					r.UserId,
					r.User.UserName,
					UserFullName = r.User.FullName,
					UserProfilePic = r.User.ProfilePic,
					r.TMDBId,
					MoviePosterPath = r.Movie.PosterPath,
					MovieBackdropPath = r.Movie.BackdropPath,
					MovieTitle = r.Movie.Title,
					r.Stars,
					r.RatedOn
				})
				.ToListAsync(cancellationToken);

			rateActivities = rateActivities
				.Join(userFollows,
					rate => rate.UserId,
					follow => follow.FollowId,
					(rate, follow) => new { Rate = rate, FollowedOn = follow.FollowedOn })
				.Where(x => x.Rate.RatedOn > x.FollowedOn)
				.Select(x => x.Rate)
				.ToList();

			var rateResponses = rateActivities
				.OrderByDescending(r => r.RatedOn)
				.Select(r => new FeedResponse(
					r.UserName,
					r.UserFullName,
					r.UserProfilePic,
					"rate",
					r.TMDBId.ToString(),
					r.MoviePosterPath,
					r.MovieBackdropPath,
					r.MovieTitle,
					$"rated {r.MovieTitle} with {r.Stars} stars",
					r.RatedOn
				)).ToList();

			var watchedActivities = await _context.UserWatchedMovies
				.Where(r => followedUserIds.Contains(r.UserId) && !userIdsWithHiddenActivity.Contains(r.UserId))
				.Select(r => new
				{
					r.UserId,
					r.User.UserName,
					UserFullName = r.User.FullName,
					UserProfilePic = r.User.ProfilePic,
					r.TMDBId,
					MoviePosterPath = r.Movie.PosterPath,
					MovieBackdropPath = r.Movie.BackdropPath,
					MovieTitle = r.Movie.Title,
					r.WatchedOn
				})
				.ToListAsync(cancellationToken);

			watchedActivities = watchedActivities
				.Join(userFollows,
					watched => watched.UserId,
					follow => follow.FollowId,
					(watched, follow) => new { Watched = watched, FollowedOn = follow.FollowedOn })
				.Where(x => x.Watched.WatchedOn > x.FollowedOn)
				.Select(x => x.Watched)
				.ToList();

			var watchedResponses = watchedActivities
				.OrderByDescending(r => r.WatchedOn)
				.Select(r => new FeedResponse(
					r.UserName!,
					r.UserFullName,
					r.UserProfilePic,
					"watched", 
					r.TMDBId.ToString(),
					r.MoviePosterPath,
					r.MovieBackdropPath,
					r.MovieTitle,
					$"watched {r.MovieTitle}",
					r.WatchedOn
				)).ToList();

			var allActivities = likeResponses
				.Concat(followResponses)
				.Concat(reviewResponses)
				.Concat(rateResponses)
				.Concat(watchedResponses)
				.OrderByDescending(a => a.CreatedOn)
				.ToList();

			return Result.Success<IEnumerable<FeedResponse>>(allActivities);
		}
		public async Task<Result<GetUserDetailsResponse>> GetUserDetailsAsync(string userName, CancellationToken cancellationToken = default)
		{
			var userIdToken = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
			var user = await userRepo.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
			if (user == null)
				return Result.Failure<GetUserDetailsResponse>(UserErrors.UserNotFound);

			var userFollowRepo = _unitOfWork.Repository<UserFollow>().GetQueryable();
			var followersCount = await userFollowRepo.CountAsync(uf => uf.FollowId == user.Id, cancellationToken);
			var followingCount = await userFollowRepo.CountAsync(uf => uf.UserId == user.Id, cancellationToken);

			var isFollowing = await userFollowRepo.AnyAsync(uf => uf.UserId == userIdToken && uf.FollowId == user.Id, cancellationToken);

			var response = new GetUserDetailsResponse
			(
				UserId: user.Id,
				FullName: user.FullName!,
				UserName: user.UserName!,
				Bio: user.Bio,
				ProfilePic: user.ProfilePic,
				SameUser: (user.Id == userIdToken),
				IsFollowing: isFollowing,
				FollowersCount: followersCount,
				FollowingCount: followingCount,
				NumberOfMovie: await _context.UserWatchedMovies
                    .CountAsync(w => w.UserId == user.Id, cancellationToken)
            );
			return Result.Success(response);
		}
		public async Task<Result<IEnumerable<UserRecentActivityResponse>>> GetAllRecentActivity(string userName, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
				return Result.Failure<IEnumerable<UserRecentActivityResponse>>(UserErrors.UserNotFound);

			var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			bool isOwnProfile = currentUserId == user.Id;
			if (!isOwnProfile && user.IsEnableRecentActivity)
				return Result.Success(Enumerable.Empty<UserRecentActivityResponse>());

			var likeActivities = await _context.UserLikeMovies
				.Include(l => l.User)
				.Include(l => l.Movie)
				.Where(r => r.UserId == user.Id)
				.OrderByDescending(l => l.LikedOn)
				.Select(l => new UserRecentActivityResponse
				{
					TMDBId = l.TMDBId,
					Type = "like",
					Id = l.TMDBId.ToString(),
					PosterPath = l.Movie.PosterPath,
					Name = l.Movie.Title,
					Description = $"liked {l.Movie.Title}",
					CreatedOn = l.LikedOn
				}).ToListAsync(cancellationToken);

			var watchedActivities = await _context.UserWatchedMovies
				.Include(l => l.User)
				.Include(l => l.Movie)
				.Where(r => r.UserId == user.Id)
				.OrderByDescending(l => l.WatchedOn)
				.Select(l => new UserRecentActivityResponse
				{
					TMDBId = l.TMDBId,
					Type = "Watched",
					Id = l.TMDBId.ToString(),
					PosterPath = l.Movie.PosterPath,
					Name = l.Movie.Title,
					Description = $"Watched {l.Movie.Title}",
					CreatedOn = l.WatchedOn
				}).ToListAsync(cancellationToken);

			var watchListActivities = await _context.UserMovieWatchList
				.Include(l => l.User)
				.Include(l => l.Movie)
				.Where(r => r.UserId == user.Id)
				.OrderByDescending(l => l.AddedOn)
				.Select(l => new UserRecentActivityResponse
				{
					TMDBId = l.TMDBId,
					Type = "WatchList",
					Id = l.TMDBId.ToString(),
					PosterPath = l.Movie.PosterPath,
					Name = l.Movie.Title,
					Description = $"WatchList {l.Movie.Title}",
					CreatedOn = l.AddedOn
				}).ToListAsync(cancellationToken);

			var reviewActivities = await _context.UserReviewMovies
				.Include(r => r.User)
				.Include(r => r.Movie)
				.Where(r => r.UserId == user.Id)
				.OrderByDescending(r => r.ReviewedOn)
				.Select(r => new UserRecentActivityResponse
				{
					TMDBId = r.TMDBId,
					Type = "review",
					Id = r.TMDBId.ToString(),
					PosterPath = r.Movie.PosterPath,
					Name = r.Movie.Title,
					Description = r.ReviewBody,
					CreatedOn = r.ReviewedOn
				}).ToListAsync(cancellationToken);

			var rateActivities = await _context.UserRateMovies
				.Include(r => r.User)
				.Include(r => r.Movie)
				.Where(r => r.UserId == user.Id)
				.OrderByDescending(r => r.RatedOn)
				.Select(r => new UserRecentActivityResponse
				{
					TMDBId = r.TMDBId,
					Type = "rate",
					Id = r.TMDBId.ToString(),
					PosterPath = r.Movie.PosterPath,
					Name = r.Movie.Title,
					Stars = r.Stars,
					Description = $"rated {r.Movie.Title} with {r.Stars} stars",
					CreatedOn = r.RatedOn
				}).ToListAsync(cancellationToken);

			var allActivities = likeActivities
				.Concat(watchListActivities)
				.Concat(watchedActivities)
				.Concat(reviewActivities)
				.Concat(rateActivities)
				.OrderByDescending(a => a.CreatedOn)
				.ToList();
			return Result.Success<IEnumerable<UserRecentActivityResponse>>(allActivities);
		}
		public async Task<Result> ToggleFollowerAndFollowing(string userName, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
				return Result.Failure(UserErrors.UserNameNotFound);
			user.IsEnableFollowerAndFollowing = !user.IsEnableFollowerAndFollowing;
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
		public async Task<Result> ToggleRecentActivity(string userName, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
				return Result.Failure(UserErrors.UserNameNotFound);
			user.IsEnableRecentActivity = !user.IsEnableRecentActivity;
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
		public async Task<Result> ToggleNotificationFollowing(string userName, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
				return Result.Failure(UserErrors.UserNameNotFound);
			user.IsEnableNotificationFollowing = !user.IsEnableNotificationFollowing;
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
		public async Task<Result> ToggleNotificationNewRelease(string userName, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
				return Result.Failure(UserErrors.UserNameNotFound);
			user.IsEnableNotificationNewRelease = !user.IsEnableNotificationNewRelease;
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
		public async Task<Result<PrivacyResponse>> GetPrivacyAsync(string userName, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
				return Result.Failure<PrivacyResponse>(UserErrors.UserNameNotFound);

			var privacyResponse = new PrivacyResponse
			(
				IsEnableFollowerAndFollowing: user.IsEnableFollowerAndFollowing,
				IsEnableRecentActivity: user.IsEnableRecentActivity
			);
			return Result.Success(privacyResponse);
		}
		public async Task<Result<NotificationPrivacyResponse>> GetNotificationPrivacy(string userName, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByNameAsync(userName);
			if (user is null)
				return Result.Failure<NotificationPrivacyResponse>(UserErrors.UserNameNotFound);

			var privacyResponse = new NotificationPrivacyResponse
			(
				IsEnableNotificationFollowing: user.IsEnableNotificationFollowing,
				IsEnableNotificationNewRelease: user.IsEnableNotificationNewRelease
			);
			return Result.Success(privacyResponse);
		}
		public async Task<Result<IEnumerable<MoviesTopTenResponse>>> CalculateUserTestAsync(string userName, CancellationToken cancellationToken = default)
		{
			try
			{
				var user = await _userManager.FindByNameAsync(userName);
				if (user == null)
					return Result.Failure<IEnumerable<MoviesTopTenResponse>>(UserErrors.UserNotFound);

				var age = DateTime.UtcNow.Year - user.BirthDay.Year;
				if (user.BirthDay > DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-age)))
					age--;

				var requestBody = new
				{
					user_id = user.Id,
					gender = user.Gender,
					age = age,
					occupation = (int)(user.JobTypes ?? JobTypes.Other)
				};

				var jsonContent = JsonSerializer.Serialize(requestBody);
				var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync(MLMovieRecommenderUrl, content, cancellationToken);
				var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

				if (response.IsSuccessStatusCode)
				{
					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
						AllowTrailingCommas = true,
						ReadCommentHandling = JsonCommentHandling.Skip
					};

					var mlResult = JsonSerializer.Deserialize<TestMLResponse>(responseJson, options)
						?? throw new JsonException($"Deserialization failed. Response: {responseJson}");

					if (mlResult.Status != "success" || mlResult.Data?.TestList == null || !mlResult.Data.TestList.Any())
						return Result.Failure<IEnumerable<MoviesTopTenResponse>>(MovieErrors.MLRecommendationNoResults);

					var movieDetails = await _context.Movies
						.Where(m => mlResult.Data.TestList.Contains(m.TMDBId) && !m.IsDeleted)
						.Select(m => new MoviesTopTenResponse(
							m.TMDBId,
							m.Title,
							m.PosterPath,
							m.IMDBRating,
							m.MPA
						))
						.ToListAsync(cancellationToken);

					var orderedMovies = mlResult.Data.TestList
						.Select(id => movieDetails.FirstOrDefault(m => m.TMDBId == id))
						.Where(m => m != null)
						.ToList();

					return Result.Success<IEnumerable<MoviesTopTenResponse>>(orderedMovies);
				}
				else
					return Result.Failure<IEnumerable<MoviesTopTenResponse>>(MovieErrors.MLRecommendationNoResults);
			}
			catch (Exception ex)
			{
				return Result.Failure<IEnumerable<MoviesTopTenResponse>>(MovieErrors.MLRecommendationError);
			}
		}
		//public async Task<Result> TestMLRecommendationFlowAsync(string userName, List<MovieRatingItem> ratings, CancellationToken cancellationToken = default)
		//{
		//	try
		//	{
		//		var recommendationsResult = await CalculateUserTestAsync(userName, cancellationToken);
		//		if (!recommendationsResult.IsSuccess)
		//			return Result.Failure(recommendationsResult.Error);

		//		var movieIds = recommendationsResult.Value;
		//		var saveRatingsResult = await _userRateMovieService.SaveMovieRatingsAsync(ratings, cancellationToken);
		//		if (!saveRatingsResult.IsSuccess)
		//			return Result.Failure(UserErrors.TestSaveRatingsFailed);

		//		return Result.Success();
		//	}
		//	catch (Exception ex)
		//	{
		//		return Result.Failure(UserErrors.TestSaveRatingsFailed);
		//	}
		//}
		private string GetBaseUrl(string subFolder)
		{
			var request = _httpContextAccessor.HttpContext?.Request;
			if (request == null)
				throw new InvalidOperationException("HttpContext is not available.");

			return $"{request.Scheme}://{request.Host}/images/{subFolder}/";
		}
	}
	public class MLRecommendationRequest
	{
		[JsonPropertyName("userId")]
		public string UserId { get; set; } = string.Empty;

		[JsonPropertyName("age")]
		public int Age { get; set; }

		[JsonPropertyName("jobType")]
		public int JobType { get; set; }
	}
	public class MLRecommendationResponse
	{
		[JsonPropertyName("movie_ids")]
		public List<int> MovieIds { get; set; } = new List<int>();
	}
	public class TestMLResponse
	{
		public TestMLData? Data { get; set; }
		public string? Message { get; set; }
		public string Status { get; set; } = string.Empty;
	}
	public class TestMLData
	{
		[JsonPropertyName("test_list")]
		public List<int> TestList { get; set; } = new List<int>();
	}
}