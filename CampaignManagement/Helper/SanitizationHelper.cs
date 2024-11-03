using System.Text.RegularExpressions;

namespace CampaignManagement.Helper
{
    public static class SanitizationHelper
    {
        /// <summary>
        /// Sanitizes a string input by removing unwanted characters.
        /// </summary>
        /// <param name="input">The input string to sanitize.</param>
        /// <returns>The sanitized string.</returns>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            // Remove any non-alphanumeric characters (except spaces)
            var sanitized = Regex.Replace(input, @"[^\w\s]", "");
            return sanitized.Trim(); // Also trim leading/trailing whitespace
        }
    }
}
