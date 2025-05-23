using Azure;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Service.Services.User_Review_Movie
{
    public class UserReviewMovieService : IUserReviewMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserReviewMovieService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

		public async Task<OperationResult> AddUserReviewMovieAsync(UserReviewMovieResponse request, CancellationToken cancellationToken = default)
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
				var entity = new UserReviewMovie
				{
					UserId = userId,
					TMDBId = request.TMDBId,
					ReviewedOn = DateTime.Now,
					ReviewBody = request.ReviewBody
				};

				await _unitOfWork.Repository<UserReviewMovie>().AddAsync(entity);
				await _unitOfWork.CompleteAsync();

				return OperationResult.Success("Review added successfully.");
			}
			catch (Exception ex)
			{
				return OperationResult.Failure($"Failed to add review: {ex.Message}");
			}
		}

		public async Task<OperationResult> DeleteUserReviewMovieAsync(UserReviewDeleteMovieResponse request, CancellationToken cancellationToken = default)
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

				var review = await _unitOfWork.Repository<UserReviewMovie>()
					.GetQueryable()
					.FirstOrDefaultAsync(r =>
						r.ReviewId == request.ReviewId &&
						r.TMDBId == request.TMDBId &&
						r.UserId == userId,
						cancellationToken);

				if (review == null)
					return OperationResult.Failure("Review not found.");

				_unitOfWork.Repository<UserReviewMovie>().Delete(review);
				await _unitOfWork.CompleteAsync();

				return OperationResult.Success("Review deleted successfully.");
			}
			catch (Exception ex)
			{
				return OperationResult.Failure($"Failed to delete review: {ex.Message}");
			}
		}

		public async Task<IEnumerable<UserReviewMovieResponseBack>> GetUserReviewMoviesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<UserReviewMovie>()
                        .GetQueryable() // Assuming this returns IQueryable<UserLikeMovie>
                        .Include(ul => ul.User)
                        .Include(ul => ul.Movie)
						.ThenInclude(m => m.UserRates)
						.Select(ul => new UserReviewMovieResponseBack
        {
                            UserId = ul.UserId,
                            ReviewId = ul.ReviewId,
							ReviewBody = ul.ReviewBody,
                            Title = ul.Movie.Title,
                            TMDBId = ul.Movie.TMDBId,
                            Poster_path = ul.Movie.PosterPath,
							Stars = ul.Movie.UserRates.Where(urate => urate.UserId == ul.UserId).Select(urate => urate.Stars).FirstOrDefault() ?? 0
						})
                        .ToListAsync(cancellationToken);

          
        }
    }
}
