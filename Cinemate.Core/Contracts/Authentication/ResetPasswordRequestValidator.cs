using Cinemate.Core.Abstractions.Consts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
	public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(x => x.Code)
				.NotEmpty();

			RuleFor(x => x.NewPassword)
				.NotEmpty()
				.Matches(RegexPatterns.Password)
				.WithMessage("Passwrod should be at least 6 digits and should contains lower case, nonalphanumeric and uppercase");

		}
	}
}
