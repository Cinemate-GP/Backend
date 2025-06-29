using FluentValidation;

namespace Cinemate.Core.Contracts.Notifications
{
    public class GetNotificationsRequestValidator : AbstractValidator<GetNotificationsRequest>
    {
        public GetNotificationsRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100");
        }
    }
}
