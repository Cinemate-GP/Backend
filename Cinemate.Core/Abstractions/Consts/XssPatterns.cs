using System.Text.RegularExpressions;

namespace Cinemate.Core.Abstractions.Consts
{
    public static class XssPatterns
    {
        // Common XSS attack patterns
        public static readonly string[] DangerousPatterns = {
            "<script",
            "</script>",
            "javascript:",
            "vbscript:",
            "data:",
            "onload=",
            "onerror=",
            "onclick=",
            "onmouseover=",
            "onfocus=",
            "onblur=",
            "onchange=",
            "onsubmit=",
            "onkeydown=",
            "onkeyup=",
            "onkeypress=",
            "onmousedown=",
            "onmouseup=",
            "onmousemove=",
            "onmouseout=",
            "oncontextmenu=",
            "ondblclick=",
            "ondrag=",
            "ondrop=",
            "onselect=",
            "onscroll=",
            "onresize=",
            "onabort=",
            "oncanplay=",
            "oncanplaythrough=",
            "oncuechange=",
            "ondurationchange=",
            "onemptied=",
            "onended=",
            "onloadeddata=",
            "onloadedmetadata=",
            "onloadstart=",
            "onpause=",
            "onplay=",
            "onplaying=",
            "onprogress=",
            "onratechange=",
            "onseeked=",
            "onseeking=",
            "onstalled=",
            "onsuspend=",
            "ontimeupdate=",
            "onvolumechange=",
            "onwaiting=",
            "ontoggle=",
            "eval(",
            "expression(",
            "url(",
            "alert(",
            "confirm(",
            "prompt(",
            "settimeout(",
            "setinterval(",
            "\\\\u00",
            "&#x",
            "&#",
            "%3c",
            "%3e",
            "%22",
            "%27",
            "%3d",
            "%3C",
            "%3E",
            "%2F",
            "%28",
            "%29",
            "xss",
            "XSS",
            "<iframe",
            "<embed",
            "<object",
            "<link",
            "<meta",
            "<base",
            "<form",
            "<input",
            "<img",
            "<svg",
            "<math",
            "src=",
            "href=",
            "action=",
            "formaction=",
            "background=",
            "cite=",
            "codebase=",
            "data=",
            "dynsrc=",
            "longdesc=",
            "profile=",
            "usemap=",
            "classid=",
            "poster="
        };

        // Regex patterns for more sophisticated detection
        public static readonly Regex ScriptTagRegex = new(@"<\s*script[^>]*>.*?<\s*/\s*script\s*>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
        public static readonly Regex HtmlTagRegex = new(@"<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex JavaScriptRegex = new(@"javascript\s*:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex VbScriptRegex = new(@"vbscript\s*:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex DataUrlRegex = new(@"data\s*:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex OnEventRegex = new(@"on\w+\s*=", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex ExpressionRegex = new(@"expression\s*\(", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex EvalRegex = new(@"eval\s*\(", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex AlertRegex = new(@"alert\s*\(", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex ConfirmRegex = new(@"confirm\s*\(", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex PromptRegex = new(@"prompt\s*\(", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
        // URL encoded patterns
        public static readonly Regex UrlEncodedRegex = new(@"(%3c|%3e|%22|%27|%3d|%2f|%28|%29)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
        // Unicode encoded patterns
        public static readonly Regex UnicodeRegex = new(@"(\\u00[0-9a-f]{2}|&#x[0-9a-f]+;|&#[0-9]+;)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
        // Base64 patterns that might contain XSS
        public static readonly Regex Base64XssRegex = new(@"(?:PHNjcmlwdA|c2NyaXB0|YWxlcnQ|dmFyIChkb2N1bWVudA)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
        // Hexadecimal patterns
        public static readonly Regex HexEncodedRegex = new(@"\\x[0-9a-f]{2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
        // CSS expression patterns
        public static readonly Regex CssExpressionRegex = new(@"expression\s*\([^)]*\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
          // Import/include patterns
        public static readonly Regex ImportRegex = new(@"@import\s*[""']?[^""']*[""']?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
        // Iframe/object/embed patterns
        public static readonly Regex DangerousTagsRegex = new(@"<\s*(iframe|object|embed|script|img|svg|link|meta|base|form)[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
