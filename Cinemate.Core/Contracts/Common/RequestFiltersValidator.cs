using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Common
{
    public class RequestFiltersValidator : BaseValidator<RequestFilters>
    {
        public RequestFiltersValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            When(x => !string.IsNullOrWhiteSpace(x.SearchValue), () => {
                WithXssProtection(RuleFor(x => x.SearchValue!)
                    .Length(1, 200)
                    .WithMessage("Search value must be between 1 and 200 characters."));
            });

            When(x => !string.IsNullOrWhiteSpace(x.Gener), () => {
                WithXssProtection(RuleFor(x => x.Gener!)
                    .Length(1, 50)
                    .WithMessage("Genre must be between 1 and 50 characters."));
            });

            When(x => !string.IsNullOrWhiteSpace(x.Year), () => {
                WithXssProtection(RuleFor(x => x.Year!)
                    .Matches(@"^\d{4}$")
                    .WithMessage("Year must be a valid 4-digit year."));
            });

            When(x => !string.IsNullOrWhiteSpace(x.MPA), () => {
                WithXssProtection(RuleFor(x => x.MPA!)
                    .Length(1, 10)
                    .WithMessage("MPA rating must be between 1 and 10 characters."));
            });

            RuleFor(x => x.SortDirection)
                .IsInEnum()
                .WithMessage("Sort direction must be a valid value.");
        }
    }
}
