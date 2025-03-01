using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Helpers
{
    public class MailSetting
    {
		public static string SectionName = "MailSettings";
		public string Mail { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
