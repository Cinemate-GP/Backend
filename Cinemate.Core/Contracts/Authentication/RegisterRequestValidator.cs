using Cinemate.Core.Abstractions.Consts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 characters and should contain a lowercase, uppercase, number, and a special character.");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 100);

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
