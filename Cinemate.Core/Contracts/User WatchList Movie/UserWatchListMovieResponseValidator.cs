using Cinemate.Core.Contracts.Common;
using Cinemate.Core.Contracts.User_Like;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_WatchList_Movie
{
    public class UserWatchListMovieResponseValidator : BaseValidator<UserWatchListMovieResponse>
    {
        public UserWatchListMovieResponseValidator()
        {
            WithXssProtection(RuleFor(x => x.UserId)
                .NotEmpty()
                .Length(3, 100));

            RuleFor(x => x.TMDBId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
