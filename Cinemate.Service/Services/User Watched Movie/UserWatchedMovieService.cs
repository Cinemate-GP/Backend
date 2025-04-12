using Cinemate.Core.Contracts.User_Watched_Movie;
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

namespace Cinemate.Service.Services.User_Watched_Movie
{
    internal class UserWatchedMovieService : IUserWatchedMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserWatchedMovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> AddUserWatchedMovieAsync(UserWatchedMovieResponse userWatchedMovieResponse, CancellationToken cancellationToken = default)
        {
            try
            {
                // Map the response DTO to entity (assuming you have a UserWatchedMovie entity)
                var entity = new UserWatchedMovie
                {
                    UserId = userWatchedMovieResponse.UserId,
                    MovieId = userWatchedMovieResponse.MovieId,
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
                            .FirstOrDefault(l => l.MovieId == response.MovieId && l.UserId == response.UserId);

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

        public async Task<IEnumerable<UserWatchedMovie>> GetUserWatchedMoviesAsync(CancellationToken cancellationToken = default)
        {
            var Watched = await _unitOfWork.Repository<UserWatchedMovie>().GetAllAsync();

            // Return the mapped response, not the original `Watched`
            return Watched;
        }
    }
}
