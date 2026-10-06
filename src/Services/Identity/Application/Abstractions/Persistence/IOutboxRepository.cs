namespace Vendora.Services.Identity.Application.Abstractions.Persistence;

public interface IOutboxRepository
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
    public int RetryCount { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public string? Error { get; set; }
}