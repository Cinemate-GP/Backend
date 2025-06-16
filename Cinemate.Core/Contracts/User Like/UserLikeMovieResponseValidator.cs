using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Like
{
    public class UserLikeMovieResponseValidator: BaseValidator<UserLikeMovieResponse>
    {
        public UserLikeMovieResponseValidator()
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
