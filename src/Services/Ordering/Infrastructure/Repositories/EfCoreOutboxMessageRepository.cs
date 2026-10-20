using System.Text.Json;
using Vendora.BuildingBlocks.Messaging.Abstractions;
using Vendora.Services.Ordering.Application.IntegrationEvents;

namespace Vendora.Services.Ordering.Infrastructure.Repositories;

public sealed class EfCoreOutboxMessageRepository(
    ApplicationDbContext context,
    IEventTypeRegistry registry) : IOutboxMessageRepository
{
    public void Add<TIntegrationEvent>(TIntegrationEvent @event, DateTimeOffset createdAt) where TIntegrationEvent : IIntegrationEvent
    {
        var payload = JsonSerializer.Serialize(@event);
        context.OutboxMessages.Add(new OutboxMessage
        {
            Id = Guid.CreateVersion7(),
            Type = registry[typeof(OrderPlacedEvent)],
            Payload = payload,
            CreatedAt = createdAt
        });
    }

    public Task<List<OutboxMessage>> ListAsync(int buffer, CancellationToken ct)
    {
        return context.OutboxMessages
            .Where(message => message.ProcessedAt == null)
            .OrderBy(message => message.CreatedAt)
            .ThenBy(message => message.Id)
            .Take(buffer)
            .ToListAsync(ct);
    }
}