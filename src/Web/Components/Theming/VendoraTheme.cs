using MudBlazor;

namespace Vendora.Web.Components.Theming;

public static class VendoraTheme
{
    public static MudTheme Create() => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2563EB",
            PrimaryContrastText = "#FFFFFF",
            Secondary = "#1E6B3A",
            SecondaryContrastText = "#FFFFFF",
            Background = "#F8FAFC",
            Surface = "#FFFFFF",
            TextPrimary = "#0F172A",
            TextSecondary = "#475569",
            ActionDefault = "#475569",
            Divider = "#E2E8F0",
            LinesDefault = "#CBD5E1",
            Error = "#DC2626",
            Warning = "#7A5300",
            Success = "#1E6B3A",
            AppbarBackground = "#FFFFFF",
            AppbarText = "#0F172A"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#60A5FA",
            PrimaryContrastText = "#0F172A",
            Secondary = "#8DD9A4",
            SecondaryContrastText = "#0F172A",
            Background = "#0B0F19",
            Surface = "#111827",
            TextPrimary = "#F8FAFC",
            TextSecondary = "#94A3B8",
            ActionDefault = "#94A3B8",
            Divider = "#1E293B",
            LinesDefault = "#334155",
            Error = "#F87171",
            Warning = "#F3C46B",
            Success = "#8DD9A4",
            AppbarBackground = "#0B0F19",
            AppbarText = "#F8FAFC"
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "10px"
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Inter", "system-ui", "-apple-system", "sans-serif"]
            },
            Button = new ButtonTypography
            {
                TextTransform = "none",
                FontWeight = "600",
                LetterSpacing = "0.01em"
            }
        }
    };
}
