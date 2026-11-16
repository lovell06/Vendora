using Vendora.Web.Features.Products;

namespace Vendora.Web.Features.Home;

// Presentation fixtures only. Replace this source with the catalog Gateway when available.
internal static class DemoCatalog
{
    internal sealed record Product(string Id, string Name, string Category, string PriceText, string ImageUrl, bool InStock = true)
    {
        public IReadOnlyList<ProductImage> Gallery { get; init; } = [new("primary", ImageUrl, Name)];
        public IReadOnlyList<ProductVariant> Variants { get; init; } = [];

        public string Description => Id switch
        {
            "headphones" => "A comfortable over-ear design for your everyday listening. A simple wireless companion for music, work and quiet moments.",
            "speaker" => "A compact speaker designed to move with you. Its portable shape fits comfortably on a shelf, desk or in your bag.",
            "backpack" => "A versatile canvas backpack with a spacious main compartment and a front pocket for everyday essentials.",
            "keyboard" => "A compact layout that keeps your workspace clear. A clean, understated design for your everyday desk setup.",
            "bottle" => "A reusable insulated bottle with a simple screw-top lid. Designed to fit into your daily routine.",
            _ => "An adjustable desk lamp with a compact base and a clean silhouette for a considered workspace."
        };
    }
    internal static readonly Product[] Products =
    [
        new("headphones", "Studio wireless headphones", "Audio", "1.290.000 ₫", "images/demo/headphones.svg")
        {
            Gallery =
            [
                new("blue-front", "images/demo/headphones.svg", "Midnight blue · Front view"),
                new("blue-detail", "images/demo/headphones-blue-detail.svg", "Midnight blue · Ear cushion detail"),
                new("graphite-front", "images/demo/headphones-graphite.svg", "Graphite · Front view"),
                new("graphite-detail", "images/demo/headphones-graphite-detail.svg", "Graphite · Ear cushion detail"),
                new("sand-front", "images/demo/headphones-sand.svg", "Sand · Front view"),
                new("sand-detail", "images/demo/headphones-sand-detail.svg", "Sand · Ear cushion detail")
            ],
            Variants =
            [
                new("blue", "Midnight blue", "HP-BLUE", ["blue-front", "blue-detail"], "blue-front"),
                new("graphite", "Graphite", "HP-GRAPHITE", ["graphite-front", "graphite-detail"], "graphite-front"),
                new("sand", "Sand", "HP-SAND", ["sand-front", "sand-detail"], "sand-front", false)
            ]
        },
        new("speaker", "Portable Bluetooth speaker", "Audio", "890.000 ₫", "images/demo/speaker.svg"),
        new("backpack", "Everyday canvas backpack", "Accessories", "590.000 ₫", "images/demo/backpack.svg"),
        new("keyboard", "Compact wireless keyboard", "Workspace", "1.490.000 ₫", "images/demo/keyboard.svg"),
        new("bottle", "Insulated everyday bottle", "Accessories", "320.000 ₫", "images/demo/bottle.svg", false),
        new("lamp", "Adjustable desk lamp", "Workspace", "750.000 ₫", "images/demo/lamp.svg")
    ];
}
