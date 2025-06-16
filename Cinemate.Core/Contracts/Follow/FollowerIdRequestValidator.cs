using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Follow
{
    public class FollowerIdRequestValidator : BaseValidator<FollowerIdRequest>
    {
        public FollowerIdRequestValidator()
        {
            WithXssProtection(RuleFor(x => x.Id)
                .NotEmpty()
                .Length(3, 100)
                .WithMessage("Id is required and must be between 3 and 100 characters."));
        }
    }
}
