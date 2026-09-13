using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendora.Services.Catalog.Domain.Categories;

namespace Vendora.Services.Catalog.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(cat => cat.Id);

        builder.Property(cat => cat.Id)
            .HasColumnName("id");

        builder.Property(cat => cat.Name)
            .HasColumnName("name")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(cat => cat.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(cat => cat.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(cat => cat.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.HasMany(cat => cat.Products)
            .WithOne(p => p.Category);
    }
}