using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
	public class ForgetPasswrodRequestValidator : BaseValidator<ForgetPasswrodRequest>
	{
		public ForgetPasswrodRequestValidator()
		{
			WithXssProtection(RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress());
		}
	}
}
