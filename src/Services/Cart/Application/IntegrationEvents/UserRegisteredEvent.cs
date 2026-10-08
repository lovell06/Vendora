namespace Vendora.Services.Cart.Application.IntegrationEvents;

public sealed class UserRegisteredEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public Guid UserId { get; init; }
    public required string UserEmail { get; init; }
}