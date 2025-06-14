using Cinemate.Core.Contracts.User_Watched_Movie;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Review_Movie
{
    public class UserReviewMovieResponseValidator : AbstractValidator<UserReviewMovieResponse>
    {
        public UserReviewMovieResponseValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .Length(3, 100);
            RuleFor(x => x.TMDBId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.ReviewBody)
                .NotEmpty()
                .Length(3, 1000)
				.Must(reviewBody => (!reviewBody.Contains("xss") && !reviewBody.Contains('<') && !reviewBody.Contains('>')))
				.WithMessage("Review body must be between 3 and 1000 characters long.");
        }
    }
}
