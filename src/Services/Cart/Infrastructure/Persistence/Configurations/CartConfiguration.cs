using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CartAggregate = Vendora.Services.Cart.Domain.Carts.Cart;

namespace Vendora.Services.Cart.Infrastructure.Persistence.Configurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<CartAggregate>
{
    public void Configure(EntityTypeBuilder<CartAggregate> builder)
    {
        builder.ToTable("carts");
        builder.HasKey(cart => cart.Id);
        builder.HasIndex(cart => cart.UserId)
            .IsUnique();

        builder.Property(cart => cart.Id)
            .HasColumnName("id");

        builder.Property(cart => cart.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(cart => cart.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(cart => cart.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.HasMany(cart => cart.Items)
            .WithOne()
            .HasForeignKey(item => item.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}