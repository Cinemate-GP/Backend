using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<OperationResult> AddUserRateMovieAsync(UserRateMovieResponse userRateMovieResponse, CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return OperationResult.Failure("Unauthorized user.");


                if (userRateMovieResponse.Stars < 0 || userRateMovieResponse.Stars > 5)
                {
                    return OperationResult.Failure("Rating must be between 1 and 5 stars.");
                }
                // Map the response DTO to entity (assuming you have a UserWatchedMovie entity)
                var entity = new UserRateMovie
                {
                    UserId = userRateMovieResponse.UserId,
                    MovieId = userRateMovieResponse.MovieId,
                    RatedOn = DateTime.UtcNow,
                    Stars = userRateMovieResponse.Stars
                };

                await _unitOfWork.Repository<UserRateMovie>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success("User Add Rated Succefully.");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Fail To Rate Movie.");
            }
        }

        public async Task<OperationResult> DeleteUserRateMovieAsync(UserRateMovieResponse response, CancellationToken cancellationToken = default)
        {
            try
            {
                var allRated = await _unitOfWork.Repository<UserRateMovie>().GetAllAsync();

                var Rated = allRated
                            .FirstOrDefault(l => l.MovieId == response.MovieId && l.UserId == response.UserId);

                if (Rated == null)
                    return OperationResult.Failure("Movie Rate not found.");

                _unitOfWork.Repository<UserRateMovie>().Delete(Rated);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Failed To Delete Rate from the Movie");
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
                            MovieId = ul.MovieId,
                            Stars=ul.Stars,
                            Title = ul.Movie.Title,
                            TMDBId = ul.Movie.TMDBId,
                            Poster_path = ul.Movie.Poster_path,
                            FullName = ul.User.FullName,
                            ProfilePic = ul.User.ProfilePic
                        })
                        .ToListAsync(cancellationToken);

            // Return the mapped response, not the original `Watched`
         
        }
    }
}
