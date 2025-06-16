using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Common
{
    public class RequestSearchValidator : BaseValidator<RequestSearch>
    {
        public RequestSearchValidator()
        {
            When(x => !string.IsNullOrWhiteSpace(x.SearchValue), () => {
                WithXssProtection(RuleFor(x => x.SearchValue!)
                    .Length(1, 200)
                    .WithMessage("Search value must be between 1 and 200 characters."));
            });
        }
    }
}
