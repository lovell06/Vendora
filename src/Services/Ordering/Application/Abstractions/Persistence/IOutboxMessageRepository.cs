using Vendora.BuildingBlocks.Messaging.Abstractions;

namespace Vendora.Services.Ordering.Application.Abstractions.Persistence;

public interface IOutboxMessageRepository
{
    void Add<TIntegrationEvent>(TIntegrationEvent @event, DateTimeOffset createdAt) 
        where TIntegrationEvent : IIntegrationEvent;
    Task<List<OutboxMessage>> ListAsync(int buffer, CancellationToken ct);
}

public class OutboxMessage
{
    public Guid Id { get; init; }
    public required string Type { get; init; }
    public required string Payload { get; init; }
    public int RetryCount { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? ProcessedAt { get; private set; }
    public string? Error { get; set; }

    public void Process(
        DateTimeOffset processedAt,
        string? error = null)
    {
        if (error is null)
            ProcessedAt = processedAt;
        else
            Error = error;
        
        ++RetryCount;
    }
}