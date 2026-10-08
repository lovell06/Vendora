namespace Vendora.Services.Identity.Application.IntegrationEvents;

public sealed record UserRegisteredEvent(
    Guid Id,
    DateTimeOffset OccurredAt,
    Guid UserId,
    string UserEmail) : IIntegrationEvent;