using Cinemate.Core.Contracts.Common;
using FluentValidation;

namespace Cinemate.Core.Contracts.Authentication
{
	public class LoginRequestValidator : BaseValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			WithXssProtection(RuleFor(x => x.UserNameOrEmail)
				.NotEmpty());

			WithXssProtection(RuleFor(x => x.Password)
				.NotEmpty());
		}
	}
}
