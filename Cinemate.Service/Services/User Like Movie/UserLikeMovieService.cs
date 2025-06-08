using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cinemate.Service.Services.User_Like_Movie
{
    public class UserLikeMovieService : IUserLikeMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLikeMovieService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> AddUserLikeMovieAsync(UserLikeMovieResponse request, CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return OperationResult.Failure("Unauthorized user.");

				if (!string.IsNullOrEmpty(request.UserId) && request.UserId != userId)
				{
					var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
					var requestedUser = await userRepo.FirstOrDefaultAsync(u => u.UserName == request.UserId, cancellationToken);

					if (requestedUser != null)
						userId = requestedUser.Id;
				}

				var existingLikedItem = await _unitOfWork.Repository<UserLikeMovie>()
				.GetQueryable()
					.FirstOrDefaultAsync(w => w.UserId == userId && w.TMDBId == request.TMDBId, cancellationToken);

				if (existingLikedItem is not null)
					return OperationResult.Success("Movie already in Liked Movie before.");

				var entity = new UserLikeMovie
                {
                    UserId = userId,
                    TMDBId = request.TMDBId,
                    LikedOn = DateTime.UtcNow
                };

                await _unitOfWork.Repository<UserLikeMovie>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success("User added like successfully.");
            }
            catch (Exception ex)
            {
                // Log ex if needed
				return OperationResult.Failure($"Failed to add like: {ex.Message}");
            }
        }
		public async Task<OperationResult> DeleteUserLikeMovieAsync(UserLikeMovieResponse request, CancellationToken cancellationToken = default)
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

				var like = await _unitOfWork.Repository<UserLikeMovie>().GetQueryable().FirstOrDefaultAsync(l => l.TMDBId == request.TMDBId && l.UserId == userId, cancellationToken);

                if (like == null)
                    return OperationResult.Failure("Movie like not found.");

                _unitOfWork.Repository<UserLikeMovie>().Delete(like);
                await _unitOfWork.CompleteAsync();
				return OperationResult.Success("Movie unliked successfully.");
            }
            catch (Exception ex)
            {
				return OperationResult.Failure($"Failed to delete like from the movie: {ex.Message}");
            }
        }
        public async Task<IEnumerable<UserLikeMovieResponseBack>> GetUserLikeMoviesAsync(CancellationToken cancellationToken = default)
        {
            
            return await _unitOfWork.Repository<UserLikeMovie>()
                        .GetQueryable() // Assuming this returns IQueryable<UserLikeMovie>
                        .Include(ul => ul.User)
                        .Include(ul => ul.Movie)
                        .Select(ul => new UserLikeMovieResponseBack
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
