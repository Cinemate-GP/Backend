using Cinemate.Core.Contracts.Profile;
using Cinemate.Core.Entities;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
    public interface IProfileService
    {
        Task<OperationResult> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default);
        Task<OperationResult> DeleteAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable< UserWatchedMovie>> GetAllMoviesWatched(CancellationToken cancellationToken = default);
        Task<IEnumerable< UserLikeMovie>> GetAllMoviesLiked(CancellationToken cancellationToken = default);
        Task< IEnumerable<UserRateMovie>> GetAllMoviesRated(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserReviewMovie>> GetAllMoviesReviews(CancellationToken cancellationToken = default);




    }
}
