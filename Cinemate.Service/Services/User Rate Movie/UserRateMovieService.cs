using Cinemate.Core.Contracts.User_Like;
using Cinemate.Core.Contracts.User_Rate_Movie;
using Cinemate.Core.Contracts.User_Watched_Movie;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Cinemate.Repository.Errors.Authentication.AuthenticationError;

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

				var existingRating = await _unitOfWork.Repository<UserRateMovie>()
					.GetQueryable()
					.FirstOrDefaultAsync(r => r.UserId == userRateMovieResponse.UserId &&
											 r.TMDBId == userRateMovieResponse.TMDBId,
											 cancellationToken);

				if (existingRating != null)
				{
					existingRating.Stars = userRateMovieResponse.Stars;
					existingRating.RatedOn = DateTime.UtcNow;

					await _unitOfWork.Repository<UserRateMovie>().Update(existingRating);
					await _unitOfWork.CompleteAsync();

					return OperationResult.Success("User rating updated successfully.");
				}
				else
				{
					var entity = new UserRateMovie
					{
						UserId = userRateMovieResponse.UserId,
						TMDBId = userRateMovieResponse.TMDBId,
						RatedOn = DateTime.UtcNow,
						Stars = userRateMovieResponse.Stars
					};

					await _unitOfWork.Repository<UserRateMovie>().AddAsync(entity);
					await _unitOfWork.CompleteAsync();

					return OperationResult.Success("User rating added successfully.");
				}
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
                            .FirstOrDefault(l => l.TMDBId == response.TMDBId && l.UserId == response.UserId);

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
                            Stars=ul.Stars,
                            Title = ul.Movie.Title,
                            TMDBId = ul.Movie.TMDBId,
                            Poster_path = ul.Movie.PosterPath,
                            FullName = ul.User.FullName,
                            ProfilePic = ul.User.ProfilePic
                        })
                        .ToListAsync(cancellationToken);

            // Return the mapped response, not the original `Watched`
         
        }
		public async Task<Result<IEnumerable<UserRateMovieResponseBack>>> GetMoviesRatedByUserAsync(string userId, CancellationToken cancellationToken = default)
		{
			var userDetails = await _unitOfWork.Repository<ApplicationUser>().GetQueryable().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
			if (userDetails is null)
				return Result.Failure<IEnumerable<UserRateMovieResponseBack>>(UserErrors.UserNotFound);

			var userRateMovieRepo = _unitOfWork.Repository<UserRateMovie>().GetQueryable();
			var movieRepo = _unitOfWork.Repository<Movie>().GetQueryable();
			var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();

			var ratedMovies = await (from rate in userRateMovieRepo
									 join movie in movieRepo on rate.TMDBId equals movie.TMDBId
									 join user in userRepo on rate.UserId equals user.Id
									 select new UserRateMovieResponseBack
									 {
										 UserId = rate.UserId,
										 TMDBId = rate.TMDBId,
										 Stars = rate.Stars,
										 Title = movie.Title,
										 Poster_path = movie.PosterPath,
										 FullName = user.FullName,
										 ProfilePic = user.ProfilePic,
										 CreatedAt = rate.RatedOn
									 }).ToListAsync(cancellationToken);

			return Result.Success<IEnumerable<UserRateMovieResponseBack>>(ratedMovies);
		}
	}
}
