using Cinemate.Core.Contracts.User_Like;
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
    public interface IUserLikeMovieService
    {
        Task<OperationResult> AddUserLikeMovieAsync(UserLikeMovieResponse userLikeMovieResponse, CancellationToken cancellationToken = default);

        Task<OperationResult> DeleteUserLikeMovieAsync(UserLikeMovieResponse response, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserLikeMovie>> GetUserLikeMoviesAsync(CancellationToken cancellationToken = default);




    }
}
