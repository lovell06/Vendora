namespace Vendora.Services.Order.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(i => new { i.OrderId, i.ProductId });

        builder.Property(i => i.OrderId)
            .HasColumnName("order_id");

        builder.Property(i => i.ProductId)
            .HasColumnName("product_id");

        builder.Property(i => i.ProductName)
            .HasColumnName("product_name")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(i => i.UnitPrice)
            .HasColumnName("unit_price")
            .IsRequired();
    }
}
