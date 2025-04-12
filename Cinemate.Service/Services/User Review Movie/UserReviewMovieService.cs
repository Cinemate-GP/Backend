using Azure;
using Cinemate.Core.Contracts.User_Review_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Service.Services.User_Review_Movie
{
    public class UserReviewMovieService : IUserReviewMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserReviewMovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> AddUserReviewMovieAsync(UserReviewMovieResponse userReviewMovieResponse, CancellationToken cancellationToken = default)
        {
            try
            {
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

        public async Task<IEnumerable<UserReviewMovie>> GetUserReviewMoviesAsync(CancellationToken cancellationToken = default)
        {
            var Reviewed = await _unitOfWork.Repository<UserReviewMovie>().GetAllAsync();

            return Reviewed;
        }
    }
}
