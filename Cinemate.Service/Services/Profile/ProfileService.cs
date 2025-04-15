using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Contracts.Profile;
using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Analytics.UserId;
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


        

        public ProfileService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IFileService fileService, IHttpContextAccessor httpContextAccessor, IUserLikeMovieService userLikeMovieService, IUserRateMovieService userRateMovieService, IUserReviewMovieService userReviewMovieService, IUserWatchedMovieService userWatchedMovieService, IUserWatchlistMovieService userWatchlistService)
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

        public async Task<OperationResult> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return OperationResult.Failure("User is not authenticated.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return OperationResult.Failure("User not found.");

            // Update full name
            if(request.FullName != null)
            user.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                var emailResult = await _userManager.SetEmailAsync(user, request.Email);
                if (!emailResult.Succeeded)
                {
                    var errors = string.Join(", ", emailResult.Errors.Select(e => e.Description));
                    return OperationResult.Failure($"Email update failed: {errors}");
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
                        return OperationResult.Failure($"Password update failed: {errors}");
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
                    return OperationResult.Failure($"Profile update failed: {errors}");
                }

                return OperationResult.Success();

            }
        
   
        private string GetBaseUrl(string subFolder)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                throw new InvalidOperationException("HttpContext is not available.");

            return $"{request.Scheme}://{request.Host}/images/{subFolder}/";
        }



    }
}
