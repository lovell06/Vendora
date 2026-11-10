namespace Vendora.Services.Catalog.Infrastructure.Persistence.Configurations;

public sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("product_variants");
        builder.HasKey(variant => variant.Id);
        builder.HasIndex(variant => variant.Sku)
            .IsUnique();

        builder.Property(variant => variant.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(variant => variant.Name)
            .HasColumnName("name")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(variant => variant.Sku)
            .HasColumnName("sku")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(variant => variant.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(variant => variant.Price)
            .HasColumnName("price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(variant => variant.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(variant => variant.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(variant => variant.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);
    }
}