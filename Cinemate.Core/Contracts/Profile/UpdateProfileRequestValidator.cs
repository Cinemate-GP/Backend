using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Contracts.Files;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Profile
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(x => x.FullName)
				    .Must(fullname => (!fullname.Contains("xss") && !fullname.Contains('<') && !fullname.Contains('>')))
					.WithMessage("invalid xss request")
                    .Length(3, 100);

			RuleFor(x => x.UserName)
					.Must(username => username == null || (!username.Contains('.') && !username.Contains('_') && !username.Contains('@') && !username.Contains("xss") && !username.Contains('<') && !username.Contains('>')))
					.WithMessage("Username must not contain (.) or (_) or (@)");

            RuleFor(x => x.Bio)
                    .Must(bio => (!bio.Contains("xss") && !bio.Contains('<') && !bio.Contains('>')))
					.WithMessage("invalid xss request");

			RuleFor(x => x.Email)
			   .Must(email => (!email.Contains("xss") && !email.Contains('<') && !email.Contains('>')))
			   .WithMessage("invalid xss request")
			   .EmailAddress();

            RuleFor(x => x.Password)   
               .Matches(RegexPatterns.Password)
			   .Must(password => (!password.Contains("xss") && !password.Contains('<') && !password.Contains('>')))
               .WithMessage("Password should be at least 6 characters and should contain a lowercase, uppercase, number, and a special character.");
            
            RuleFor(x => x.Profile_Image)
                .SetValidator(new BlockedSignaturesValidator()!)
                .SetValidator(new FileSizeValidator()!);
        }
    }
}
