using System.Globalization;

namespace Vendora.Web.Gateway;

public static class PriceFormatter
{
    private static readonly CultureInfo Vietnamese = new("vi-VN");

    public static string Format(decimal amount, string currency) =>
        string.Equals(currency, "VND", StringComparison.OrdinalIgnoreCase)
            ? string.Create(Vietnamese, $"{amount:N0} ₫")
            : string.Create(CultureInfo.InvariantCulture, $"{amount:N2} {currency.ToUpperInvariant()}");

    public static string FormatRange(decimal min, decimal max, string currency) =>
        min == max ? Format(min, currency) : $"{Format(min, currency)} – {Format(max, currency)}";
}
