using FluentValidation;

namespace Cinemate.Core.Contracts.Authentication
{
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x.UserNameOrEmail)
				.NotEmpty();

			RuleFor(x => x.Password)
				.NotEmpty();
		}
	}
}
