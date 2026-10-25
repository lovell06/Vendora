namespace Vendora.Services.Ordering.Infrastructure.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(message => message.Id);

        builder.Property(message => message.Id)
            .HasColumnName("id");

        builder.Property(message => message.Type)
            .HasColumnName("type")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(message => message.Payload)
            .HasColumnName("payload")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(message => message.AttemptCount)
            .HasColumnName("attempt_count");

        builder.Property(message => message.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(message => message.ProcessedAt)
            .HasColumnName("processed_at")
            .IsRequired(false);

        builder.Property(message => message.LastError)
            .HasColumnName("last_error")
            .HasMaxLength(256)
            .IsRequired(false);

        builder.HasIndex(message => new { message.CreatedAt, message.Id });
    }
}