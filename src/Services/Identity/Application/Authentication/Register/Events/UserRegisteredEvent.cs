namespace Vendora.Services.Identity.Application.Authentication.Register.Events;

public sealed record UserRegisteredEvent(
    Guid Id,
    DateTimeOffset OccurredAt,
    Guid UserId,
    string UserEmail) : IIntegrationEvent;