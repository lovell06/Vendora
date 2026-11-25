using Vendora.Web.Features.Admin.Products.Shared;

namespace Vendora.Web.Features.Admin.Products.Create;

public sealed class VariantRow
{
    public string Sku { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public HashSet<int> ImageIndexes { get; set; } = [];
}

// The new-product form model. Scoped to the circuit so a session-expiry redirect to /login and back
// (same tab) does not lose what was typed. Images stay in memory only; nothing is written to storage.
public sealed class ProductDraft
{
    public int Step { get; set; }
    public string Name { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Description { get; set; } = "";
    public int CategoryId { get; set; }
    public bool IsVisible { get; set; } = true;
    public List<ImageItem> Images { get; } = [];
    public List<VariantRow> Variants { get; } = [new()];

    public bool IsDirty => Name.Length > 0 || Brand.Length > 0 || Description.Length > 0 || Images.Count > 0
        || Variants.Any(v => v.Sku.Length > 0 || v.Name.Length > 0);

    public void Reset()
    {
        Step = 0; Name = Brand = Description = ""; CategoryId = 0; IsVisible = true;
        Images.Clear(); Variants.Clear(); Variants.Add(new VariantRow());
    }
}
