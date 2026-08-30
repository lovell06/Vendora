using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendora.Services.Identity.Domain.Users;

namespace Vendora.Services.Identity.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        /*
         * Table configuring
         */
        builder.ToTable("users");
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.Email)
            .IsUnique();

        /*
         * Field configuring
         */
        builder.Property(user => user.Id)
            .HasColumnName("id");
        
        builder.Property(user => user.Email)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(user => user.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(user => user.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(user => user.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();
        
        builder.Property(user => user.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();
        
        builder.Property(user => user.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.Property(user => user.UpdatedAt)
            .HasColumnName("updated_at");
        
        builder.Property(user => user.EmailVerifiedAt)
            .HasColumnName("email_verified_at");
    }
}