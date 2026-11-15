using MudBlazor;

namespace Vendora.Web.Components.Theming;

public static class VendoraTheme
{
    public static MudTheme Create() => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#285EA8", PrimaryContrastText = "#FFFFFF",
            Background = "#F8FAFD", Surface = "#FFFFFF",
            TextPrimary = "#1A1C20", TextSecondary = "#454B56",
            ActionDefault = "#454B56", Divider = "#C5CDDA",
            LinesDefault = "#747C89", Error = "#A32B25",
            AppbarBackground = "#F8FAFD", AppbarText = "#1A1C20"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#AAC7FF", PrimaryContrastText = "#102F59",
            Background = "#111318", Surface = "#191C22",
            TextPrimary = "#E2E5ED", TextSecondary = "#C2C8D4",
            ActionDefault = "#C2C8D4", Divider = "#424A57",
            LinesDefault = "#8C95A4", Error = "#FFB4AA",
            AppbarBackground = "#111318", AppbarText = "#E2E5ED"
        },
        LayoutProperties = new LayoutProperties { DefaultBorderRadius = "4px" },
        Typography = new Typography
        {
            Default = new DefaultTypography { FontFamily = ["Inter", "Segoe UI", "sans-serif"] },
            Button = new ButtonTypography { TextTransform = "none", FontWeight = "600" }
        }
    };
}
