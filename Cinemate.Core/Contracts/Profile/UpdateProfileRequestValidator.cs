using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Contracts.Common;
using Cinemate.Core.Contracts.Files;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Profile
{
    public class UpdateProfileRequestValidator : BaseValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            When(x => !string.IsNullOrWhiteSpace(x.FullName), () => {
                WithXssProtection(RuleFor(x => x.FullName!)
                    .Length(3, 100));
            });

            When(x => !string.IsNullOrWhiteSpace(x.UserName), () => {
                WithXssProtection(RuleFor(x => x.UserName!)
                    .Must(username => (!username.Contains('.') && !username.Contains('_') && !username.Contains('@')))
                    .WithMessage("Username must not contain (.) or (_) or (@)"));
            });

            When(x => !string.IsNullOrWhiteSpace(x.Bio), () => {
                WithXssProtection(RuleFor(x => x.Bio!));
            });

            When(x => !string.IsNullOrWhiteSpace(x.Email), () => {
                WithXssProtection(RuleFor(x => x.Email!)
                    .EmailAddress());
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () => {
                WithXssProtection(RuleFor(x => x.Password!)
                    .Matches(RegexPatterns.Password)
                    .WithMessage("Password should be at least 6 characters and should contain a lowercase, uppercase, number, and a special character."));
            });
            
            RuleFor(x => x.Profile_Image)
                .SetValidator(new BlockedSignaturesValidator()!)
                .SetValidator(new FileSizeValidator()!)
                .When(x => x.Profile_Image != null);
        }
    }
}
