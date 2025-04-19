using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Entities;
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

        public async Task<OperationResult> AddUserWatchedMovieAsync(UserWatchedMovieResponse userWatchedMovieResponse, CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return OperationResult.Failure("Unauthorized user.");
                // Map the response DTO to entity (assuming you have a UserWatchedMovie entity)
                var entity = new UserWatchedMovie
                {
                    UserId = userWatchedMovieResponse.UserId,
                    TMDBId = userWatchedMovieResponse.TMDBId,
                    WatchedOn = DateTime.UtcNow
                };

                await _unitOfWork.Repository<UserWatchedMovie>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success("User Add Watched Succefully.");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Fail To Add Watched.");
            }
        }

        public async Task<OperationResult> DeleteUserWatchedMovieAsync(UserWatchedMovieResponse response, CancellationToken cancellationToken = default)
        {
            try
            {
                // For example, delete by UserId + MovieId if they’re unique together
                var allWatched = await _unitOfWork.Repository<UserWatchedMovie>().GetAllAsync();

                // Filter Watched by MovieId and UserId
                var Watched = allWatched
                            .FirstOrDefault(l => l.TMDBId == response.TMDBId && l.UserId == response.UserId);

                if (Watched == null)
                    return OperationResult.Failure("Movie Watched not found.");

                _unitOfWork.Repository<UserWatchedMovie>().Delete(Watched);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Failed To Delete Watched from the Movie");
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
