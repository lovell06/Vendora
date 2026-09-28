using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendora.Services.Cart.Domain.Carts;

namespace Vendora.Services.Cart.Infrastructure.Persistence.Configurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("cart_items");
        builder.HasKey(item => new { item.CartId, item.ProductId });

        builder.Property(item => item.CartId)
            .HasColumnName("cart_id")
            .IsRequired();

        builder.Property(item => item.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(item => item.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(item => item.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(item => item.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);
    }
}