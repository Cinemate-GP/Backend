using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Service.Services.User_Watched_Movie
{
    internal class UserWatchedMovieService : IUserWatchedMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserWatchedMovieService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

		public async Task<OperationResult> AddUserWatchedMovieAsync(UserWatchedMovieResponse request, CancellationToken cancellationToken = default)
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
				var existingWatchedlistItem = await _unitOfWork.Repository<UserWatchedMovie>()
					.GetQueryable()
					.FirstOrDefaultAsync(w => w.UserId == userId && w.TMDBId == request.TMDBId, cancellationToken);

				if (existingWatchedlistItem is not null)
					return OperationResult.Success("Movie already watched before.");

				var entity = new UserWatchedMovie
				{
					UserId = userId,
					TMDBId = request.TMDBId,
					WatchedOn = DateTime.Now
				};

				await _unitOfWork.Repository<UserWatchedMovie>().AddAsync(entity);
				await _unitOfWork.CompleteAsync();

				return OperationResult.Success("Movie marked as watched successfully.");
			}
			catch (Exception ex)
			{
				return OperationResult.Failure($"Failed to mark movie as watched: {ex.Message}");
			}
		}

		public async Task<OperationResult> DeleteUserWatchedMovieAsync(UserWatchedMovieResponse request, CancellationToken cancellationToken = default)
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
				var watchedMovie = await _unitOfWork.Repository<UserWatchedMovie>()
					.GetQueryable()
					.FirstOrDefaultAsync(w => w.TMDBId == request.TMDBId && w.UserId == userId, cancellationToken);

				if (watchedMovie == null)
					return OperationResult.Failure("Movie not found in watched list.");

				_unitOfWork.Repository<UserWatchedMovie>().Delete(watchedMovie);
				await _unitOfWork.CompleteAsync();

				return OperationResult.Success("Movie removed from watched list successfully.");
			}
			catch (Exception ex)
			{
				return OperationResult.Failure($"Failed to remove movie from watched list: {ex.Message}");
			}
		}

		public async Task<IEnumerable<UserWatchedMovieResponseBack>> GetUserWatchedMoviesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<UserWatchedMovie>()
                       .GetQueryable() // Assuming this returns IQueryable<UserLikeMovie>
                       .Include(ul => ul.User)
                       .Include(ul => ul.Movie)
                       .Select(ul => new UserWatchedMovieResponseBack
        {
                           UserId = ul.UserId,
                           Title = ul.Movie.Title,
                           TMDBId = ul.Movie.TMDBId,
                           Poster_path = ul.Movie.PosterPath,
                           FullName = ul.User.FullName,
                           ProfilePic = ul.User.ProfilePic
                       })
                       .ToListAsync(cancellationToken);
        }
    }
}
