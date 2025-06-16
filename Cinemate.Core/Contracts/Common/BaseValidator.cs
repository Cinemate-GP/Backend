using Cinemate.Core.Helpers;
using FluentValidation;

namespace Cinemate.Core.Contracts.Common
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected IRuleBuilderOptions<T, string> WithXssProtection(IRuleBuilder<T, string> rule)
        {
            return rule.Must(value => XssHelper.IsInputSafe(value))
                      .WithMessage(XssHelper.GetXssErrorMessage());
        }
        protected IRuleBuilderOptions<T, string> WithStrictXssProtection(IRuleBuilder<T, string> rule)
        {
            return rule.Must(value => XssHelper.IsSafeForHtml(value))
                      .WithMessage(XssHelper.GetXssErrorMessage());
        }
        protected IRuleBuilderOptions<T, string?> WithOptionalXssProtection(IRuleBuilder<T, string?> rule)
        {
            return rule.Must(value => XssHelper.IsInputSafe(value))
                      .WithMessage(XssHelper.GetXssErrorMessage());
        }
        protected IRuleBuilderOptions<T, string> WithUrlSafeXssProtection(IRuleBuilder<T, string> rule)
        {
            return rule.Must(value => XssHelper.IsSafeForUrl(value))
                      .WithMessage("Invalid URL detected. The URL contains potentially dangerous content.");
        }
        protected IRuleBuilderOptions<T, string?> WithOptionalUrlSafeXssProtection(IRuleBuilder<T, string?> rule)
        {
            return rule.Must(value => XssHelper.IsSafeForUrl(value))
                      .WithMessage("Invalid URL detected. The URL contains potentially dangerous content.");
        }
    }
}
