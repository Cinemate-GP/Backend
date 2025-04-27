using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_WatchList_Movie;
using Cinemate.Core.Entities;
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

		public async Task<OperationResult> AddUserWatchlistMovieAsync(UserWatchListMovieResponse userWatchlistMovieResponse, CancellationToken cancellationToken = default)
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

				if (string.IsNullOrEmpty(userId))
					return OperationResult.Failure("Unauthorized user.");

				var existingWatchlistItem = await _unitOfWork.Repository<UserMovieWatchList>()
					.GetQueryable()
					.FirstOrDefaultAsync(w => w.UserId == userId && w.TMDBId == userWatchlistMovieResponse.TMDBId, cancellationToken);

				if (existingWatchlistItem is not null)
				    return OperationResult.Success("movie already in watchlist before.");

				var entity = new UserMovieWatchList
				{
					UserId = userId,
					TMDBId = userWatchlistMovieResponse.TMDBId,
					AddedOn = DateTime.UtcNow
				};

				await _unitOfWork.Repository<UserMovieWatchList>().AddAsync(entity);
				await _unitOfWork.CompleteAsync();

				return OperationResult.Success("User added Watchlist successfully.");
			}
			catch (Exception ex)
			{
				// Log ex if needed
				return OperationResult.Failure("Failed to add To WatchList.");
			}
		}

		public async Task<OperationResult> DeleteUserWatchlistMovieAsync(UserWatchListMovieResponse response, CancellationToken cancellationToken = default)
        {
            try
            {
                // For example, delete by UserId + MovieId if they’re unique together
                var AllWatchlist = await _unitOfWork.Repository<UserMovieWatchList>().GetAllAsync();

                // Filter likes by MovieId and UserId
                var watchList = AllWatchlist
                            .FirstOrDefault(l => l.TMDBId == response.TMDBId && l.UserId == response.UserId);

                if (watchList == null)
                    return OperationResult.Failure("Movie in the watchlist not found.");

                _unitOfWork.Repository<UserMovieWatchList>().Delete(watchList);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Failed To Delete movie from the Watchlist");
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
