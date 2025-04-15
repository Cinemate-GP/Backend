using Azure;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
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

        public async Task<OperationResult> AddUserReviewMovieAsync(UserReviewMovieResponse userReviewMovieResponse, CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return OperationResult.Failure("Unauthorized user.");
                var entity = new UserReviewMovie
                {
                    UserId = userReviewMovieResponse.UserId,
                    MovieId = userReviewMovieResponse.MovieId,
                    ReviewedOn = DateTime.UtcNow,
                    ReviewBody = userReviewMovieResponse.ReviewBody
                };

                await _unitOfWork.Repository<UserReviewMovie>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success("User Add Review Succefully.");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Fail To Add Review.");
            }
        }

        public async Task<OperationResult> DeleteUserReviewMovieAsync(UserReviewDeleteMovieResponse response, CancellationToken cancellationToken = default)
        {
            try
            {
                var Reviewes = await _unitOfWork.Repository<UserReviewMovie>().GetAllAsync();

                var review = Reviewes
                             .FirstOrDefault(l =>
                            l.ReviewId == response.ReviewId &&
                            l.MovieId == response.MovieId &&
                            l.UserId == response.UserId);
                if (review == null)
                    return OperationResult.Failure("Movie Reviewed not found.");

                _unitOfWork.Repository<UserReviewMovie>().Delete(review);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Failed To Delete Review from the Movie");
            }
        }

        public async Task<IEnumerable<UserReviewMovieResponseBack>> GetUserReviewMoviesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<UserReviewMovie>()
                        .GetQueryable() // Assuming this returns IQueryable<UserLikeMovie>
                        .Include(ul => ul.User)
                        .Include(ul => ul.Movie)
                        .Select(ul => new UserReviewMovieResponseBack
        {
                            UserId = ul.UserId,
                            MovieId = ul.MovieId,
                            ReviewBody = ul.ReviewBody,
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
