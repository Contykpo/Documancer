namespace Documancer.Infrastructure.Constants.Localization
{
    public static class LocalizationConstants
    {
        public const string ResourcesPath = "Resources";

        public static readonly LanguageCode[] SupportedLanguages =
        {
        new()
        {
            Code = "en-US",
            DisplayName = "English"
        },
        new()
        {
            Code = "es-AR",
            DisplayName = "Spanish"
        },
    };
    }

    public class LanguageCode
    {
        public string DisplayName { get; set; } = "en-US";
        public string Code { get; set; } = "English";
    }
}