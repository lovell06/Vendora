namespace Vendora.Web.Localization;

// Marker type for IStringLocalizer<SharedResource>. Keys are the English UI text, so English needs no
// resource file (a missing key falls back to itself). Translations live in SharedResource.<culture>.resx.
public sealed class SharedResource;
