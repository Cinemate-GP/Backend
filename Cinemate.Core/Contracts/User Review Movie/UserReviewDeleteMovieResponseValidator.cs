using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Review_Movie
{
    public class UserReviewDeleteMovieResponseValidator : BaseValidator<UserReviewDeleteMovieResponse>
    {
        public UserReviewDeleteMovieResponseValidator()
        {
            WithXssProtection(RuleFor(x => x.UserId)
                .NotEmpty()
                .Length(3, 100));
            
            RuleFor(x => x.ReviewId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.TMDBId)
                .NotEmpty()
                .GreaterThan(0);
           
        }
    }
   
}
