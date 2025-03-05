using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
    public class ResendConfirmationEmailRequestValidatior : AbstractValidator<ResendConfirmationEmailRequest>
    {
        public ResendConfirmationEmailRequestValidatior()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
