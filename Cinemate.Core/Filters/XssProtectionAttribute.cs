using Cinemate.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cinemate.Core.Filters
{
    /// <summary>
    /// Action filter to validate route parameters for XSS attacks
    /// </summary>
    public class XssProtectionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check route values for XSS
            foreach (var routeValue in context.RouteData.Values)
            {
                if (routeValue.Value is string stringValue && !string.IsNullOrEmpty(stringValue))
                {
                    if (!XssHelper.IsInputSafe(stringValue))
                    {
                        context.Result = new BadRequestObjectResult(new
                        {
                            error = "Invalid input detected in route parameters",
                            message = XssHelper.GetXssErrorMessage()
                        });
                        return;
                    }
                }
            }

            // Check query parameters for XSS
            foreach (var queryParam in context.HttpContext.Request.Query)
            {
                foreach (var value in queryParam.Value)
                {
                    if (!string.IsNullOrEmpty(value) && !XssHelper.IsInputSafe(value))
                    {
                        context.Result = new BadRequestObjectResult(new
                        {
                            error = "Invalid input detected in query parameters",
                            message = XssHelper.GetXssErrorMessage()
                        });
                        return;
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
