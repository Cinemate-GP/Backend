using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
    public class ConfirmEmailRequestValidator : BaseValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator()
        {
            WithXssProtection(RuleFor(x => x.UserId)
               .NotEmpty());

            WithXssProtection(RuleFor(x => x.Code)
                .NotEmpty());
        }
    }
}
