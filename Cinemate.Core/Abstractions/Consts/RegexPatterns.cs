using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Abstractions.Consts
{
    public static class RegexPatterns
    {
        public const string Password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";

    }
}
