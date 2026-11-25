using System.Globalization;
using System.Resources;

namespace Vendora.Web.Localization;

// Translation lookup for code that is not a component (services, validation attributes).
// Uses the same SharedResource files as IStringLocalizer<SharedResource>; the key is the English text.
public static class Text
{
    private static readonly ResourceManager Resources = new(typeof(SharedResource).FullName!, typeof(SharedResource).Assembly);

    public static string Get(string english)
    {
        try { return Resources.GetString(english, CultureInfo.CurrentUICulture) ?? english; }
        catch (MissingManifestResourceException) { return english; }
    }

    public static string Format(string english, params object?[] args) =>
        string.Format(CultureInfo.CurrentCulture, Get(english), args);
}
