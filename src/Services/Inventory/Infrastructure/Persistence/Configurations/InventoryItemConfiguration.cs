namespace Vendora.Services.Inventory.Infrastructure.Persistence.Configurations;

public sealed class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("inventory_items");
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .HasColumnName("id");

        builder.Property(item => item.ProductId)
            .HasColumnName("product_id");

        builder.Property(item => item.ProductVariantId)
            .HasColumnName("product_variant_id");

        builder.Property(item => item.Sku)
            .HasColumnName("sku")
            .HasMaxLength(20);

        builder.Property(item => item.OnHandQuantity)
            .HasColumnName("on_hand_quantity")
            .IsRequired();

        builder.Property(item => item.ReservedQuantity)
            .HasColumnName("reserved_quantity")
            .IsRequired();

        builder.Property(item => item.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(item => item.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.HasMany(item => item.Adjustments)
            .WithOne()
            .HasForeignKey(adjustment => adjustment.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}