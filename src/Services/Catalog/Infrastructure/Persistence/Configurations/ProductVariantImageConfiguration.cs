namespace Vendora.Services.Catalog.Infrastructure.Persistence.Configurations;

public sealed class ProductVariantImageConfiguration : IEntityTypeConfiguration<ProductVariantImage>
{
    public void Configure(EntityTypeBuilder<ProductVariantImage> builder)
    {
        builder.ToTable("product_variant_images");
        builder.HasKey(image => image.Id);

        builder.HasIndex(image => new
        {
            image.ProductVariantId,
            image.ProductImageId
        }).IsUnique();

        builder.HasIndex(image => new
        {
            image.ProductVariantId,
            image.SortOrder
        }).IsUnique();

        builder.HasIndex(image => image.ProductVariantId)
            .HasDatabaseName("ux_product_variant_images_primary")
            .IsUnique()
            .HasFilter("is_primary = TRUE");

        builder.Property(image => image.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(image => image.ProductVariantId)
            .HasColumnName("product_variant_id")
            .IsRequired();

        builder.Property(image => image.ProductImageId)
            .HasColumnName("product_image_id")
            .IsRequired();

        builder.Property(image => image.IsPrimary)
            .HasColumnName("is_primary")
            .IsRequired();

        builder.Property(image => image.SortOrder)
            .HasColumnName("sort_order")
            .IsRequired();

        builder.HasOne(variantImage => variantImage.ProductImage)
            .WithMany()
            .HasForeignKey(variantImage => variantImage.ProductImageId);

        builder.HasOne(variantImage => variantImage.ProductVariant)
            .WithMany(productImage => productImage.ImageAssignments)
            .HasForeignKey(variantImage => variantImage.ProductVariantId);
    }
}