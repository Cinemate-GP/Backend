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
                .EmailAddress()
                .Must(email => (!email.Contains("xss") && !email.Contains('<') && !email.Contains('>')))
                .WithMessage("invalid xss request");


			RuleFor(x => x.Password)
                .NotEmpty()
				.Must(password => (!password.Contains("xss") && !password.Contains('<') && !password.Contains('>')))
                .WithMessage("invalid xss request")
				.Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 6 characters and should contain a lowercase, uppercase, number, and a special character.");

            RuleFor(x => x.FullName)
                .NotEmpty()
				.Length(3, 100)
                .Must(fullname => (!fullname.Contains("xss") && !fullname.Contains('<') && !fullname.Contains('>')))
				.WithMessage("invalid xss request");


			RuleFor(x => x.UserName)
					.Must(username => (!username.Contains('.') && !username.Contains('_') && !username.Contains('@') && !username.Contains("xss") && !username.Contains('<') && !username.Contains('>')))
					.WithMessage("Username must not contain (.) or (_) or (@)");

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
