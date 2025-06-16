using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
	public class ResetPasswordRequestValidator : BaseValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidator()
		{
			WithXssProtection(RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress());

			WithXssProtection(RuleFor(x => x.Code)
				.NotEmpty());

			WithXssProtection(RuleFor(x => x.NewPassword)
				.NotEmpty()
				.Matches(RegexPatterns.Password)
				.WithMessage("Password should be at least 6 digits and should contain lower case, nonalphanumeric and uppercase"));
		}
	}
}
