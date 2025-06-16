using Cinemate.Core.Helpers;
using Xunit;

namespace Cinemate.Tests.Security
{
    public class XssProtectionTests
    {
        [Theory]
        [InlineData("<script>alert('xss')</script>", false)]
        [InlineData("javascript:alert('xss')", false)]
        [InlineData("<img src=x onerror=alert('xss')>", false)]
        [InlineData("onclick=alert('xss')", false)]
        [InlineData("eval(alert('xss'))", false)]
        [InlineData("%3Cscript%3E", false)]
        [InlineData("&#60;script&#62;", false)]
        [InlineData("Hello World", true)]
        [InlineData("user@example.com", true)]
        [InlineData("Regular text with numbers 123", true)]
        [InlineData("", true)]
        [InlineData(null, true)]
        public void IsInputSafe_ShouldDetectXssAttacks(string input, bool expectedSafe)
        {
            // Act
            var result = XssHelper.IsInputSafe(input);

            // Assert
            Assert.Equal(expectedSafe, result);
        }

        [Theory]
        [InlineData("<script>alert('xss')</script>")]
        [InlineData("<img src=x onerror=alert('xss')>")]
        [InlineData("<div onclick='alert()'>text</div>")]
        public void IsSafeForHtml_ShouldRejectHtmlTags(string input)
        {
            // Act
            var result = XssHelper.IsSafeForHtml(input);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("javascript:alert('xss')")]
        [InlineData("vbscript:alert('xss')")]
        [InlineData("data:text/html,<script>alert()</script>")]
        public void IsSafeForUrl_ShouldRejectDangerousProtocols(string input)
        {
            // Act
            var result = XssHelper.IsSafeForUrl(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SanitizeInput_ShouldRemoveDangerousContent()
        {
            // Arrange
            var input = "<script>alert('xss')</script>Hello World";

            // Act
            var result = XssHelper.SanitizeInput(input);

            // Assert
            Assert.DoesNotContain("<script>", result);
            Assert.DoesNotContain("alert", result);
            Assert.Contains("Hello World", result);
        }

        [Theory]
        [InlineData("<div>Hello</div>", "&lt;div&gt;Hello&lt;/div&gt;")]
        [InlineData("'quote' and \"doublequote\"", "&#x27;quote&#x27; and &quot;doublequote&quot;")]
        public void EncodeForHtml_ShouldEncodeSpecialCharacters(string input, string expected)
        {
            // Act
            var result = XssHelper.EncodeForHtml(input);

            // Assert
            Assert.Contains("Hello", result);
            Assert.DoesNotContain("<div>", result);
        }
    }
}
