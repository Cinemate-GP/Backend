using Cinemate.Core.Abstractions.Consts;
using System.Text.RegularExpressions;
using System.Web;

namespace Cinemate.Core.Helpers
{
    public static class XssHelper
    {
        /// <summary>
        /// Validates if the input contains potential XSS attacks
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <returns>True if input is safe, false if it contains XSS patterns</returns>
        public static bool IsInputSafe(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            var normalizedInput = input.ToLowerInvariant();

            // Check for basic dangerous patterns
            foreach (var pattern in XssPatterns.DangerousPatterns)
            {
                if (normalizedInput.Contains(pattern.ToLower()))
                    return false;
            }

            // Check regex patterns
            if (XssPatterns.ScriptTagRegex.IsMatch(input) ||
                XssPatterns.JavaScriptRegex.IsMatch(input) ||
                XssPatterns.VbScriptRegex.IsMatch(input) ||
                XssPatterns.DataUrlRegex.IsMatch(input) ||
                XssPatterns.OnEventRegex.IsMatch(input) ||
                XssPatterns.ExpressionRegex.IsMatch(input) ||
                XssPatterns.EvalRegex.IsMatch(input) ||
                XssPatterns.AlertRegex.IsMatch(input) ||
                XssPatterns.ConfirmRegex.IsMatch(input) ||
                XssPatterns.PromptRegex.IsMatch(input) ||
                XssPatterns.UrlEncodedRegex.IsMatch(input) ||
                XssPatterns.UnicodeRegex.IsMatch(input) ||
                XssPatterns.Base64XssRegex.IsMatch(input) ||
                XssPatterns.HexEncodedRegex.IsMatch(input) ||
                XssPatterns.CssExpressionRegex.IsMatch(input) ||
                XssPatterns.ImportRegex.IsMatch(input) ||
                XssPatterns.DangerousTagsRegex.IsMatch(input))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sanitizes input by removing or encoding dangerous characters
        /// </summary>
        /// <param name="input">The input string to sanitize</param>
        /// <returns>Sanitized string</returns>
        public static string SanitizeInput(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var sanitized = input;

            // Remove script tags completely
            sanitized = XssPatterns.ScriptTagRegex.Replace(sanitized, "");
            sanitized = XssPatterns.DangerousTagsRegex.Replace(sanitized, "");

            // Encode dangerous characters
            sanitized = HttpUtility.HtmlEncode(sanitized);

            // Remove javascript: and vbscript: protocols
            sanitized = XssPatterns.JavaScriptRegex.Replace(sanitized, "");
            sanitized = XssPatterns.VbScriptRegex.Replace(sanitized, "");
            sanitized = XssPatterns.DataUrlRegex.Replace(sanitized, "");

            // Remove on* event handlers
            sanitized = XssPatterns.OnEventRegex.Replace(sanitized, "");

            // Remove CSS expressions
            sanitized = XssPatterns.CssExpressionRegex.Replace(sanitized, "");

            // Remove dangerous functions
            sanitized = XssPatterns.EvalRegex.Replace(sanitized, "");
            sanitized = XssPatterns.AlertRegex.Replace(sanitized, "");
            sanitized = XssPatterns.ConfirmRegex.Replace(sanitized, "");
            sanitized = XssPatterns.PromptRegex.Replace(sanitized, "");

            return sanitized;
        }

        /// <summary>
        /// Validates if the input is safe for use in HTML context
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <returns>True if safe for HTML, false otherwise</returns>
        public static bool IsSafeForHtml(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            // More restrictive check for HTML context
            return IsInputSafe(input) && !XssPatterns.HtmlTagRegex.IsMatch(input);
        }

        /// <summary>
        /// Validates if the input is safe for use in URLs
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <returns>True if safe for URLs, false otherwise</returns>
        public static bool IsSafeForUrl(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            // Check for URL-specific XSS patterns
            return IsInputSafe(input) && 
                   !XssPatterns.JavaScriptRegex.IsMatch(input) &&
                   !XssPatterns.VbScriptRegex.IsMatch(input) &&
                   !XssPatterns.DataUrlRegex.IsMatch(input);
        }

        /// <summary>
        /// Gets a descriptive error message for XSS validation failures
        /// </summary>
        /// <returns>User-friendly error message</returns>
        public static string GetXssErrorMessage()
        {
            return "Invalid input detected. The text contains potentially dangerous content that could be used for security attacks. Please remove any HTML tags, scripts, or special characters.";
        }

        /// <summary>
        /// Encodes output for safe display in HTML
        /// </summary>
        /// <param name="input">The input string to encode</param>
        /// <returns>HTML-encoded string</returns>
        public static string EncodeForHtml(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return HttpUtility.HtmlEncode(input);
        }

        /// <summary>
        /// Encodes output for safe use in HTML attributes
        /// </summary>
        /// <param name="input">The input string to encode</param>
        /// <returns>HTML attribute-encoded string</returns>
        public static string EncodeForHtmlAttribute(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return HttpUtility.HtmlAttributeEncode(input);
        }

        /// <summary>
        /// Encodes output for safe use in JavaScript
        /// </summary>
        /// <param name="input">The input string to encode</param>
        /// <returns>JavaScript-encoded string</returns>
        public static string EncodeForJavaScript(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return HttpUtility.JavaScriptStringEncode(input);
        }

        /// <summary>
        /// Encodes output for safe use in URLs
        /// </summary>
        /// <param name="input">The input string to encode</param>
        /// <returns>URL-encoded string</returns>
        public static string EncodeForUrl(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return HttpUtility.UrlEncode(input);
        }
    }
}
