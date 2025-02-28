using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Helpers
{
    public class EmailBodyBuilder
    {
        public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
        {
            var templatePath = $"{Directory.GetCurrentDirectory()}/Template/{template}.html";
            var streamReader = new StreamReader(templatePath);
            var body = streamReader.ReadToEnd();
            streamReader.Close();

            foreach (var item in templateModel)
                body = body.Replace(item.Key, item.Value);

            return body;
        }

    }
}
