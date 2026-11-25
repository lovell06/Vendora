namespace Vendora.Web.Gateway;

// Business details shown in the storefront footer. Empty values are simply not rendered.
public sealed class StoreInfoOptions
{
    public string Description { get; set; } = "";
    public string Hotline { get; set; } = "";
    public string Email { get; set; } = "";
    public string Hours { get; set; } = "";
    public string LegalName { get; set; } = "";
    public string TaxId { get; set; } = "";
    public string Address { get; set; } = "";
    public string Facebook { get; set; } = "";
    public string Zalo { get; set; } = "";
    public string TikTok { get; set; } = "";
    // Turn on once the static policy/support pages exist, so the footer never links to a 404.
    public bool ShowPolicyLinks { get; set; }
}
