namespace Vendora.Services.Catalog.Infrastructure.Repositories;

public sealed class EfCoreOutboxRepository(
    ApplicationDbContext context,
    IEventTypeRegistry eventTypeRegistry) : IOutboxRepository
{
    public void Add<TIntegrationEvent>(TIntegrationEvent @event, DateTimeOffset createdAt) where TIntegrationEvent : IIntegrationEvent
    {
        var typeName = eventTypeRegistry[typeof(TIntegrationEvent)];
        var payload = JsonSerializer.Serialize(@event);

        context.OutboxMessages.Add(new OutboxMessage
        {
            Id = Guid.CreateVersion7(),
            Type = typeName,
            Payload = payload,
            RetryCount = 0,
            CreatedAt = createdAt,
            ProcessedAt = null
        });
    }

    public async Task<List<OutboxMessage>> ListAsync(int buffer, CancellationToken ct)
    {
        return await context.OutboxMessages
            .Where(message => message.ProcessedAt == null)
            .OrderBy(message => message.CreatedAt)
            .Take(buffer)
            .ToListAsync(ct);
    }
}