using System.Globalization;

namespace Vendora.Web.Localization;

public static class Cultures
{
    public const string Default = "en";
    public static readonly string[] Supported = ["en", "vi"];

    public static string Current => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

    public static string DisplayName(string culture) => culture switch { "vi" => "Tiếng Việt", _ => "English" };
    public static string ShortName(string culture) => culture switch { "vi" => "VI", _ => "EN" };
}
