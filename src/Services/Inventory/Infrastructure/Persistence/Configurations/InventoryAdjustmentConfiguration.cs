using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendora.Services.Inventory.Domain.InventoryItems;

namespace Vendora.Services.Inventory.Infrastructure.Persistence.Configurations;

public sealed class InventoryAdjustmentConfiguration : IEntityTypeConfiguration<InventoryAdjustment>
{
    public void Configure(EntityTypeBuilder<InventoryAdjustment> builder)
    {
        builder.ToTable("inventory_adjustments");
        builder.HasKey(adjustment => adjustment.Id);

        builder.Property(adjustment => adjustment.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(adjustment => adjustment.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(adjustment => adjustment.QuantityChange)
            .HasColumnName("quantity_change")
            .IsRequired();

        builder.Property(adjustment => adjustment.Reason)
            .HasColumnName("reason")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(adjustment => adjustment.CreatedBy)
            .HasColumnName("created_by")
            .IsRequired();

        builder.Property(adjustment => adjustment.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne<InventoryItem>()
            .WithMany()
            .HasForeignKey(adjustment => adjustment.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(adjustment => new
            {
                adjustment.ProductId, 
                adjustment.CreatedAt,
                adjustment.Id
            })
            .IsDescending(false, true, true);
    }
}