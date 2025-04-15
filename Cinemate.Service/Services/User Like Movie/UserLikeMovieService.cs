using Cinemate.Core.Contracts.User_Like;
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

                var entity = new UserLikeMovie
                {
                    UserId = userId,
                    MovieId = request.MovieId,
                    LikedOn = DateTime.UtcNow
                };

                await _unitOfWork.Repository<UserLikeMovie>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success("User added like successfully.");
            }
            catch (Exception ex)
            {
                // Log ex if needed
                return OperationResult.Failure("Failed to add like.");
            }
        }


        public async Task<OperationResult> DeleteUserLikeMovieAsync(UserLikeMovieResponse response, CancellationToken cancellationToken = default)
        {
            try
            {
                // For example, delete by UserId + MovieId if they’re unique together
                var allLikes = await _unitOfWork.Repository<UserLikeMovie>().GetAllAsync();

                // Filter likes by MovieId and UserId
                var like = allLikes
                            .FirstOrDefault(l => l.MovieId == response.MovieId && l.UserId ==response.UserId);

                if (like == null)
                    return OperationResult.Failure("Movie like not found.");

                _unitOfWork.Repository<UserLikeMovie>().Delete(like);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Failed To Delete Like from the Movie");
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
                          MovieId = ul.MovieId,
                          Title = ul.Movie.Title,
                          TMDBId = ul.Movie.TMDBId,
                          Poster_path = ul.Movie.Poster_path,
                          FullName = ul.User.FullName,
                          ProfilePic = ul.User.ProfilePic
                         })
                        .ToListAsync(cancellationToken);

        }

    }

}
