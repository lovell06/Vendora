namespace Vendora.Services.Catalog.Infrastructure.Persistence.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("product_images");
        builder.HasKey(image => image.Id);
        builder.HasIndex(image => new { image.ProductId, image.SortOrder }).IsUnique();
        builder.HasIndex(image => image.ProductId)
            .HasDatabaseName("ux_product_images_primary")
            .IsUnique()
            .HasFilter("is_primary = TRUE");
        
        builder.Property(image => image.Id)
            .HasColumnName("id");

        builder.Property(image => image.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(image => image.ObjectKey)
            .HasColumnName("object_key")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(image => image.ContentType)
            .HasColumnName("content_type")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(image => image.SizeBytes)
            .HasColumnName("size_bytes")
            .IsRequired();

        builder.Property(image => image.IsPrimary)
            .HasColumnName("is_primary")
            .IsRequired();

        builder.Property(image => image.SortOrder)
            .HasColumnName("sort_order")
            .IsRequired();

        builder.Property(image => image.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}