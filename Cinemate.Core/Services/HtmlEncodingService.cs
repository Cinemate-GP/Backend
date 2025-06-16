using Cinemate.Core.Helpers;

namespace Cinemate.Core.Services
{
    /// <summary>
    /// Service interface for encoding output to prevent XSS attacks
    /// </summary>
    public interface IHtmlEncodingService
    {
        /// <summary>
        /// Encodes text for safe display in HTML
        /// </summary>
        string EncodeForHtml(string? input);

        /// <summary>
        /// Encodes text for safe use in HTML attributes
        /// </summary>
        string EncodeForHtmlAttribute(string? input);

        /// <summary>
        /// Encodes text for safe use in JavaScript
        /// </summary>
        string EncodeForJavaScript(string? input);

        /// <summary>
        /// Encodes text for safe use in URLs
        /// </summary>
        string EncodeForUrl(string? input);

        /// <summary>
        /// Sanitizes user input by removing dangerous content
        /// </summary>
        string SanitizeInput(string? input);
    }

    /// <summary>
    /// Implementation of HTML encoding service
    /// </summary>
    public class HtmlEncodingService : IHtmlEncodingService
    {
        public string EncodeForHtml(string? input)
        {
            return XssHelper.EncodeForHtml(input);
        }

        public string EncodeForHtmlAttribute(string? input)
        {
            return XssHelper.EncodeForHtmlAttribute(input);
        }

        public string EncodeForJavaScript(string? input)
        {
            return XssHelper.EncodeForJavaScript(input);
        }

        public string EncodeForUrl(string? input)
        {
            return XssHelper.EncodeForUrl(input);
        }

        public string SanitizeInput(string? input)
        {
            return XssHelper.SanitizeInput(input);
        }
    }
}
