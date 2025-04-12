using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Entities;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Service.Services.User_Like_Movie
{
    public class UserLikeMovieService : IUserLikeMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserLikeMovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> AddUserLikeMovieAsync(UserLikeMovieResponse request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Map the response DTO to entity (assuming you have a UserLikeMovie entity)
                var entity = new UserLikeMovie
                {
                    UserId = request.UserId,
                    MovieId = request.MovieId,
                    LikedOn = DateTime.UtcNow
                };

                await _unitOfWork.Repository<UserLikeMovie>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return OperationResult.Success("User Add Like Succefully.");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("Fail To Add Like.");
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

        public async Task<IEnumerable<UserLikeMovie>> GetUserLikeMoviesAsync(CancellationToken cancellationToken = default)
        {
            
                var likes = await _unitOfWork.Repository<UserLikeMovie>().GetAllAsync();

                // Return the mapped response, not the original `likes`
                return  likes;
            
        }

    }

}
