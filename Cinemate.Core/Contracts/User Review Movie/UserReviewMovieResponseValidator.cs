using Cinemate.Core.Contracts.Common;
using Cinemate.Core.Contracts.User_Watched_Movie;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Review_Movie
{
    public class UserReviewMovieResponseValidator : BaseValidator<UserReviewMovieResponse>
    {
        public UserReviewMovieResponseValidator()
        {
            WithXssProtection(RuleFor(x => x.UserId)
                .NotEmpty()
                .Length(3, 100));

            RuleFor(x => x.TMDBId)
                .NotEmpty()
                .GreaterThan(0);

            WithStrictXssProtection(RuleFor(x => x.ReviewBody)
                .NotEmpty()
                .Length(3, 1000)
                .WithMessage("Review body must be between 3 and 1000 characters long."));
        }
    }
}
