using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Contracts.Follow;
using Cinemate.Core.Contracts.Profile;
using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Recent_Activity;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Cinemate.Repository.Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Analytics.UserId;
using static Cinemate.Repository.Errors.Authentication.AuthenticationError;
using IFileService = Cinemate.Core.Service_Contract.IFileService;

namespace Cinemate.Service.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserLikeMovieService userLikeMovieService;
        private readonly IUserRateMovieService userRateMovieService;
        private readonly IUserReviewMovieService userReviewMovieService;
        private readonly IUserWatchedMovieService userWatchedMovieService;
        private readonly IUserWatchlistMovieService userWatchlistService;
        private readonly IUserfollowService userfollowService;
		private readonly ApplicationDbContext _context;
		
        public ProfileService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IFileService fileService, IHttpContextAccessor httpContextAccessor, IUserLikeMovieService userLikeMovieService, IUserRateMovieService userRateMovieService, IUserReviewMovieService userReviewMovieService, IUserWatchedMovieService userWatchedMovieService, IUserWatchlistMovieService userWatchlistService, IUserfollowService userfollow, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
            this.userLikeMovieService = userLikeMovieService;
            this.userRateMovieService = userRateMovieService;
            this.userReviewMovieService = userReviewMovieService;
            this.userWatchedMovieService = userWatchedMovieService;
            this.userWatchlistService = userWatchlistService;
            this.userfollowService = userfollow;
            _context = context;
        }

        public async Task<OperationResult> DeleteAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return OperationResult.Failure("User is not authenticated.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return OperationResult.Failure("User not found.");

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
            var result= await userLikeMovieService.GetUserLikeMoviesAsync(cancellationToken);
           
            return result.Where(r => r.UserId == userId);

        }

        public async Task<IEnumerable<UserRateMovieResponseBack>> GetAllMoviesRated(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserRateMovieResponseBack>();
            var result = await userRateMovieService.GetUserRateMoviesAsync(cancellationToken);

            return result.Where(r => r.UserId == userId);
        }

        public async Task<IEnumerable<UserReviewMovieResponseBack>> GetAllMoviesReviews(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserReviewMovieResponseBack>();

            var result = await userReviewMovieService.GetUserReviewMoviesAsync(cancellationToken);

            return result.Where(r => r.UserId == userId);
        }


        public async Task<IEnumerable<UserWatchedMovieResponseBack>> GetAllMoviesWatched(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserWatchedMovieResponseBack>();
            var result = await userWatchedMovieService.GetUserWatchedMoviesAsync(cancellationToken);

            return result.Where(r => r.UserId == userId);
        }
        public async Task<IEnumerable<UserWatchListMovieResponseBack>> GetAllWatchlist(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserWatchListMovieResponseBack>();
            var result = await userWatchlistService.GetUserWatchlistMoviesAsync(cancellationToken);

            return result.Where(r => r.UserId == userId);
        }

        public async Task<UpdateProfileReauestBack> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new KeyNotFoundException("User not found.");

            // Update full name
            if (request.FullName != null)
                user.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                var emailResult = await _userManager.SetEmailAsync(user, request.Email);
                if (!emailResult.Succeeded)
                {
                    var errors = string.Join(", ", emailResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Email update failed: {errors}");
                }
            }

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);

                if (!passwordResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Password update failed: {errors}");
                }
            }

            // Handle profile image upload if provided
            if (request.Profile_Image != null && request.Profile_Image.Length > 0)
            {
                // Delete old profile image if it exists
                if (!string.IsNullOrEmpty(user.ProfilePic))
                {
                    var oldFileName = Path.GetFileName(user.ProfilePic);
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Profile_Image", oldFileName);

                    // Check if the file exists in wwwroot/images/Profile_Image
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        _fileService.DeleteFile(oldFileName, ImageSubFolder.Profile_Image);
                    }
                    else
                    {
                        Console.WriteLine($"File not found at: {oldFilePath}");
                    }
                }

                string createdImageName = await _fileService.SaveFileAsync(request.Profile_Image, ImageSubFolder.Profile_Image);
                string baseUrl = GetBaseUrl("Profile_Image");
                user.ProfilePic = $"{baseUrl}{createdImageName}";
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Profile update failed: {errors}");
            }
            var Updated = new UpdateProfileReauestBack
            {
                Email = user.Email,
                Profile_Image = user.ProfilePic,
                FullName = user.FullName,

            };

            return Updated;
        }
		public async Task<Result<IEnumerable<FeedResponse>>> GetFeedForUserAsync(string id, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
				return Result.Failure<IEnumerable<FeedResponse>>(UserErrors.UserNotFound);

			var followedUserIds = await _context.UserFollows
				.Where(f => f.UserId == id)
				.Select(f => f.FollowId)
				.ToListAsync(cancellationToken);

			if (!followedUserIds.Any())
				return Result.Success(Enumerable.Empty<FeedResponse>());

			var likeActivities = await _context.UserLikeMovies
				.Include(l => l.User)
				.Include(l => l.Movie)
				.Where(l => followedUserIds.Contains(l.UserId))
				.OrderByDescending(l => l.LikedOn)
				.Select(l => new FeedResponse(
					l.UserId,
					l.User.FullName,
					l.User.ProfilePic,
					"like",
					l.TMDBId.ToString(),
					l.Movie.PosterPath,
					l.Movie.Title,
					$"liked {l.Movie.Title}",
					l.LikedOn
				)).ToListAsync(cancellationToken);

			var followActivities = await _context.UserFollows
				.Include(f => f.Follower)
				.Include(f => f.FollowedUser)
				.Where(f => followedUserIds.Contains(f.UserId))
				.OrderByDescending(f => f.FollowedOn)
				.Select(f => new FeedResponse(
					f.UserId,
					f.Follower.FullName,
					f.Follower.ProfilePic,
					"follow",
					f.FollowId,
					null,
					f.FollowedUser.FullName,
					$"followed {f.FollowedUser.FullName}",
					f.FollowedOn
				)).ToListAsync(cancellationToken);

			var reviewActivities = await _context.UserReviewMovies
				.Include(r => r.User)
				.Include(r => r.Movie)
				.Where(r => followedUserIds.Contains(r.UserId))
				.OrderByDescending(r => r.ReviewedOn)
				.Select(r => new FeedResponse(
					r.UserId,
					r.User.FullName,
					r.User.ProfilePic,
					"review",
					r.TMDBId.ToString(),
					r.Movie.PosterPath,
					r.Movie.Title,
					r.ReviewBody,
					r.ReviewedOn
				))
				.ToListAsync(cancellationToken);

			var rateActivities = await _context.UserRateMovies
				.Include(r => r.User)
				.Include(r => r.Movie)
				.Where(r => followedUserIds.Contains(r.UserId))
				.OrderByDescending(r => r.RatedOn)
				.Select(r => new FeedResponse(
					r.UserId,
					r.User.FullName,
					r.User.ProfilePic,
					"rate",
					r.TMDBId.ToString(),
					r.Movie.PosterPath,
					r.Movie.Title,
					$"rated {r.Movie.Title} with {r.Stars} stars",
					r.RatedOn
				))
				.ToListAsync(cancellationToken);

			var allActivities = likeActivities
				.Concat(followActivities)
				.Concat(reviewActivities)
				.Concat(rateActivities)
				.OrderByDescending(a => a.CreatedOn)
				.ToList();
			return Result.Success<IEnumerable<FeedResponse>>(allActivities);
		}
        public async Task<Result<IEnumerable<UserRecentActivityResponse>>> GetAllRecentActivity(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Result.Failure<IEnumerable<UserRecentActivityResponse>>(UserErrors.UserNotFound);

            var likeActivities = await _context.UserLikeMovies
                .Include(l => l.User)
                .Include(l => l.Movie)
                .Where(r => r.UserId == userId)
                .OrderByDescending(l => l.LikedOn)
                .Select(l => new UserRecentActivityResponse
                {
                    UserId = l.UserId,
                    TMDBId = l.TMDBId,
                    Type = "like",
                    Id = l.TMDBId.ToString(),
                    PosterPath = l.Movie.PosterPath,
                    Name = l.Movie.Title,
                    Description = $"liked {l.Movie.Title}",
                    CreatedOn = l.LikedOn
                }).ToListAsync(cancellationToken);

            var WatchedActivities = await _context.UserWatchedMovies
                .Include(l => l.User)
                .Include(l => l.Movie)
                .Where(r => r.UserId == userId)
                .OrderByDescending(l => l.WatchedOn)
                .Select(l => new UserRecentActivityResponse
                {
                    UserId = l.UserId,
                    TMDBId = l.TMDBId,
                    Type = "Watched",
                    Id = l.TMDBId.ToString(),
                    PosterPath = l.Movie.PosterPath,
                    Name = l.Movie.Title,
                    Description = $"Watched {l.Movie.Title}",
                    CreatedOn = l.WatchedOn
                }).ToListAsync(cancellationToken);
            var WatchListActivities = await _context.UserMovieWatchList
                .Include(l => l.User)
                .Include(l => l.Movie)
                .Where(r => r.UserId == userId)
                .OrderByDescending(l => l.AddedOn)
                .Select(l => new UserRecentActivityResponse
                {
                    UserId = l.UserId,
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
              .Where(r => r.UserId == userId)
              .OrderByDescending(r => r.ReviewedOn)
              .Select(r => new UserRecentActivityResponse
               {
                UserId = r.UserId,
                 TMDBId = r.TMDBId,
                 Type = "review",
                 Id = r.TMDBId.ToString(),
                 PosterPath = r.Movie.PosterPath,
                 Name = r.Movie.Title,
                 Description = r.ReviewBody,
                 CreatedOn = r.ReviewedOn
         
           })
      .ToListAsync(cancellationToken);


            var rateActivities = await _context.UserRateMovies
     .Include(r => r.User)
     .Include(r => r.Movie)
     .Where(r => r.UserId == userId)
     .OrderByDescending(r => r.RatedOn)
     .Select(r => new UserRecentActivityResponse
     {
         UserId = r.UserId,
         TMDBId = r.TMDBId,
         Type = "rate",
         Id = r.TMDBId.ToString(),
         PosterPath = r.Movie.PosterPath,
         Name = r.Movie.Title,
         Stars = r.Stars,
         Description = $"rated {r.Movie.Title} with {r.Stars} stars",
         CreatedOn = r.RatedOn
     })
     .ToListAsync(cancellationToken);

            var allActivities = likeActivities
                .Concat(WatchListActivities)
                .Concat(WatchedActivities)
                .Concat(reviewActivities)
                .Concat(rateActivities)
                .OrderByDescending(a => a.CreatedOn)
                .ToList();
            return Result.Success<IEnumerable<UserRecentActivityResponse>>(allActivities);

        }
        private string GetBaseUrl(string subFolder)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                throw new InvalidOperationException("HttpContext is not available.");

            return $"{request.Scheme}://{request.Host}/images/{subFolder}/";
        }

        public async Task<IEnumerable<UserDataFollow>> GetAllFollowers(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserDataFollow>();
            var result = await userfollowService.GetAllFollowers(userId,cancellationToken);

            return result.Where(r => r.UserId == userId);
        }

        public async Task<IEnumerable<UserDataFollow>> GetAllFollowing(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<UserDataFollow>();
            var result = await userfollowService.GetAllFollowing(userId, cancellationToken);

            return result.Where(r => r.UserId == userId);
        }

        public async Task<int> CountFollowers(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return 0;
            var followers = await userfollowService.GetAllFollowers(userId, cancellationToken);
            return followers.Count();
        }


        public async Task<int> CountFollowing(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return 0;
            var followers = await userfollowService.GetAllFollowing(userId, cancellationToken);
            return followers.Count();
        }

        
    }
}
