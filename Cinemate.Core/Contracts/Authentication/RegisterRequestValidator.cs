﻿using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
    public class RegisterRequestValidator : BaseValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            WithXssProtection(RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress());

            WithXssProtection(RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 6 characters and should contain a lowercase, uppercase, number, and a special character."));

            WithXssProtection(RuleFor(x => x.FullName)
                .NotEmpty()
                .Length(3, 100));

            WithXssProtection(RuleFor(x => x.UserName)
                .NotEmpty()
                .Must(username => (!username.Contains('.') && !username.Contains('_') && !username.Contains('@')))
                .WithMessage("Username must not contain (.) or (_) or (@)"));

            RuleFor(x => x.Gender)
                .NotEmpty()
                .Must(g => g.Equals("Male", StringComparison.OrdinalIgnoreCase) ||
                           g.Equals("Female", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Gender must be either 'Male' or 'Female'.");

            RuleFor(x => x.BirthDay)
                .NotEmpty();
        }
    }
}
