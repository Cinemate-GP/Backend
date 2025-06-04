using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Cinemate.Repository.Errors.Authentication.AuthenticationError;

namespace Cinemate.Service.Services.User_Rate_Movie
{
    public class UserRateMovieService : IUserRateMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRateMovieService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

		public async Task<OperationResult> AddUserRateMovieAsync(UserRateMovieResponse request, CancellationToken cancellationToken = default)
        {
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return OperationResult.Failure("Unauthorized user.");

				if (!string.IsNullOrEmpty(request.UserId))
				{
					// Check if the userId is a username
					var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
					var requestedUser = await userRepo
						.FirstOrDefaultAsync(u => u.UserName == request.UserId, cancellationToken);

					// If user found by username, use that ID
					if (requestedUser != null)
						userId = requestedUser.Id;
				}

				if (request.Stars < 0 || request.Stars > 5)
					return OperationResult.Failure("Rating must be between 0 and 5 stars.");

				var existingRating = await _unitOfWork.Repository<UserRateMovie>()
					.GetQueryable()
					.FirstOrDefaultAsync(r => r.UserId == userId && r.TMDBId == request.TMDBId, cancellationToken);

				if (existingRating != null)
				{
					existingRating.Stars = request.Stars;
					existingRating.RatedOn = DateTime.Now;

					await _unitOfWork.Repository<UserRateMovie>().Update(existingRating);
					await _unitOfWork.CompleteAsync();

					return OperationResult.Success("User rating updated successfully.");
				}
				else
				{
					var entity = new UserRateMovie
					{
						UserId = userId,
						TMDBId = request.TMDBId,
						RatedOn = DateTime.UtcNow,
						Stars = request.Stars
					};

					await _unitOfWork.Repository<UserRateMovie>().AddAsync(entity);
					await _unitOfWork.CompleteAsync();

					return OperationResult.Success("User rating added successfully.");
				}
			}
			catch (Exception ex)
            {
				return OperationResult.Failure($"Failed to rate movie: {ex.Message}");
            }
        }

		public async Task<OperationResult> DeleteUserRateMovieAsync(UserRateMovieResponse request, CancellationToken cancellationToken = default)
        {
            try
            {
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return OperationResult.Failure("Unauthorized user.");

				if (!string.IsNullOrEmpty(request.UserId))
				{
					var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
					var requestedUser = await userRepo.FirstOrDefaultAsync(u => u.UserName == request.UserId, cancellationToken);

					if (requestedUser != null)
						userId = requestedUser.Id;
				}

				var rating = await _unitOfWork.Repository<UserRateMovie>()
					.GetQueryable()
					.FirstOrDefaultAsync(r => r.TMDBId == request.TMDBId && r.UserId == userId, cancellationToken);

				if (rating == null)
					return OperationResult.Failure("Movie rating not found.");

				_unitOfWork.Repository<UserRateMovie>().Delete(rating);
                await _unitOfWork.CompleteAsync();

				return OperationResult.Success("Movie rating removed successfully.");
            }
            catch (Exception ex)
            {
				return OperationResult.Failure($"Failed to delete rating: {ex.Message}");
            }
        }

        public async Task<IEnumerable<UserRateMovieResponseBack>> GetUserRateMoviesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<UserRateMovie>()
                        .GetQueryable() // Assuming this returns IQueryable<UserLikeMovie>
                        .Include(ul => ul.User)
                        .Include(ul => ul.Movie)
                        .Select(ul => new UserRateMovieResponseBack
        {
                            UserId = ul.UserId,
                            Stars=ul.Stars,
                            Title = ul.Movie.Title,
                            TMDBId = ul.Movie.TMDBId,
                            Poster_path = ul.Movie.PosterPath,
                            FullName = ul.User.FullName,
                            ProfilePic = ul.User.ProfilePic
                        })
                        .ToListAsync(cancellationToken);

            // Return the mapped response, not the original `Watched`
         
        }
		public async Task<Result<IEnumerable<UserRateMovieResponseBack>>> GetMoviesRatedByUserAsync(string userId, CancellationToken cancellationToken = default)
		{
			var userDetails = await _unitOfWork.Repository<ApplicationUser>().GetQueryable().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
			if (userDetails is null)
				return Result.Failure<IEnumerable<UserRateMovieResponseBack>>(UserErrors.UserNotFound);

            var userRateMovie = await _unitOfWork.Repository<UserRateMovie>()
                .GetQueryable()
                .Where(ur => ur.UserId == userId)
                .Select(ur => new UserRateMovieResponseBack
									 {
					UserId = ur.UserId,
					Stars = ur.Stars,
					Title = ur.Movie.Title,
					TMDBId = ur.Movie.TMDBId,
					Poster_path = ur.Movie.PosterPath,
					FullName = ur.User.FullName,
					ProfilePic = ur.User.ProfilePic,
					CreatedAt = ur.RatedOn
									 }).ToListAsync(cancellationToken);


			return Result.Success<IEnumerable<UserRateMovieResponseBack>>(userRateMovie);
		}
	}
}
