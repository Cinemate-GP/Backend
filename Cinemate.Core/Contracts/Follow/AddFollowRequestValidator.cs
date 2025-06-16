using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Follow
{
    public class AddFollowRequestValidator : BaseValidator<AddFollowRequest>
    {
        public AddFollowRequestValidator()
        {
            WithXssProtection(RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required."));
            
            WithXssProtection(RuleFor(x => x.FollowId)
                .NotEmpty()
                .WithMessage("FollowId is required."));
        }
    }
}
