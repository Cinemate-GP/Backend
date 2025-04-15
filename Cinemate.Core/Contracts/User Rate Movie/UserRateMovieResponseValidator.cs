﻿using Cinemate.Core.Contracts.User_Watched_Movie;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Rate_Movie
{
    internal class UserRateMovieResponseValidator : AbstractValidator<UserRateMovieResponse>
    {
        public UserRateMovieResponseValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .Length(3, 100);
            RuleFor(x => x.MovieId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.Stars)
                .InclusiveBetween(1, 5);

        }
    }
}
