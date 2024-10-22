namespace Documancer.Client.Helpers
{
    public static class StringHelpers
    {
        public static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;

            if (text.Length <= maxLength) return text;

            return text.Substring(0, maxLength) + "...";
        }
    }
}
