using Cinemate.Core.Contracts.Common;
using FluentValidation;

namespace Cinemate.Core.Contracts.Authentication
{
	public class RefreshTokenRequestValidator : BaseValidator<RefreshTokenRequest>
	{
		public RefreshTokenRequestValidator()
		{
			WithXssProtection(RuleFor(x => x.Token).NotEmpty());
			WithXssProtection(RuleFor(x => x.RefreshToken).NotEmpty());
		}
	}
}
