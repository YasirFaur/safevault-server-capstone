using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace SafeVault
{
    public static class InputSanitizer
    {
        /// <summary>
        /// Sanitizes user input to prevent XSS and SQL Injection attacks.
        /// </summary>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // 1. SQLi Protection: Strip out malicious characters
            string cleanOutput = Regex.Replace(input, @"[';""--]", string.Empty);

            // 2. XSS Protection: Convert to safe HTML entities
            cleanOutput = HtmlEncoder.Default.Encode(cleanOutput);

            return cleanOutput.Trim();
        }
    }
}