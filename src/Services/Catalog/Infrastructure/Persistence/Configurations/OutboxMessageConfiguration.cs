namespace Vendora.Services.Catalog.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(message => message.Id);
        builder.HasIndex(message => new { message.CreatedAt, message.Id })
            .IsDescending(false, false);

        builder.Property(message => message.Id)
            .HasColumnName("id");
        
        builder.Property(message => message.Type)
            .HasColumnName("type")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(message => message.Payload)
            .HasColumnName("payload")
            .IsRequired();

        builder.Property(message => message.RetryCount)
            .HasColumnName("retry_count")
            .IsRequired();

        builder.Property(message => message.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(message => message.ProcessedAt)
            .HasColumnName("processed_at")
            .IsRequired(false);

        builder.Property(message => message.Error)
            .HasColumnName("error")
            .IsRequired(false);
    }
}