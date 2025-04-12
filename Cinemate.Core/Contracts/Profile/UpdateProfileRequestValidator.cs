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
                    
                    .Length(3, 100);
            RuleFor(x => x.Email)
           
               .EmailAddress();
            RuleFor(x => x.Password)
               
               .Matches(RegexPatterns.Password)
               .WithMessage("Password should be at least 6 characters and should contain a lowercase, uppercase, number, and a special character.");
            
            RuleFor(x => x.Profile_Image)
                .SetValidator(new BlockedSignaturesValidator()!)
                .SetValidator(new FileSizeValidator()!);


        }
    }
}
