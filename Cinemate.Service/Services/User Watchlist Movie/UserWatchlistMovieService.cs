using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cinemate.Service.Services.User_Watchlist_Movie
{
    public class UserWatchlistMovieService : IUserWatchlistMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserWatchlistMovieService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

		public async Task<OperationResult> AddUserWatchlistMovieAsync(UserWatchListMovieResponse request, CancellationToken cancellationToken = default)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return OperationResult.Failure("Unauthorized user.");

				if (!string.IsNullOrEmpty(request.UserId))
				{
					var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
					var requestedUser = await userRepo
						.FirstOrDefaultAsync(u => u.UserName == request.UserId, cancellationToken);

					if (requestedUser != null)
						userId = requestedUser.Id;
				}
				var existingWatchlistItem = await _unitOfWork.Repository<UserMovieWatchList>()
					.GetQueryable()
					.FirstOrDefaultAsync(w => w.UserId == userId && w.TMDBId == request.TMDBId, cancellationToken);

				if (existingWatchlistItem is not null)
					return OperationResult.Success("Movie already in watchlist.");

				var entity = new UserMovieWatchList
				{
					UserId = userId,
					TMDBId = request.TMDBId,
					AddedOn = DateTime.UtcNow
				};

				await _unitOfWork.Repository<UserMovieWatchList>().AddAsync(entity);
				await _unitOfWork.CompleteAsync();

				return OperationResult.Success("Movie added to watchlist successfully.");
			}
			catch (Exception ex)
			{
				return OperationResult.Failure($"Failed to add movie to watchlist: {ex.Message}");
			}
		}

		public async Task<OperationResult> DeleteUserWatchlistMovieAsync(UserWatchListMovieResponse request, CancellationToken cancellationToken = default)
        {
            try
            {
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
					return OperationResult.Failure("Unauthorized user.");

				if (!string.IsNullOrEmpty(request.UserId))
				{
					var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
					var requestedUser = await userRepo
						.FirstOrDefaultAsync(u => u.UserName == request.UserId, cancellationToken);

					if (requestedUser != null)
						userId = requestedUser.Id;
				}
				var watchlistItem = await _unitOfWork.Repository<UserMovieWatchList>()
					.GetQueryable()
					.FirstOrDefaultAsync(w => w.TMDBId == request.TMDBId && w.UserId == userId, cancellationToken);

				if (watchlistItem == null)
					return OperationResult.Failure("Movie not found in watchlist.");

				_unitOfWork.Repository<UserMovieWatchList>().Delete(watchlistItem);
                await _unitOfWork.CompleteAsync();

				return OperationResult.Success("Movie removed from watchlist successfully.");
            }
            catch (Exception ex)
            {
				return OperationResult.Failure($"Failed to remove movie from watchlist: {ex.Message}");
            }
        }

        public async Task<IEnumerable<UserWatchListMovieResponseBack>> GetUserWatchlistMoviesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<UserMovieWatchList>()
                      .GetQueryable() // Assuming this returns IQueryable<UserLikeMovie>
                      .Include(ul => ul.User)
                      .Include(ul => ul.Movie)
                      .Select(ul => new UserWatchListMovieResponseBack
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
