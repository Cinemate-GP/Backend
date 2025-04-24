using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Follow
{
    public class AddFollowRequestValidator:AbstractValidator<AddFollowRequest>
    {
        public AddFollowRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
            RuleFor(x => x.FollowId)
                .NotEmpty()
                .WithMessage("FollowId is required.");
        }
    }
}
