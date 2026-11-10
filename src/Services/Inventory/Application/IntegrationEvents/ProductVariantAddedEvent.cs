namespace Vendora.Services.Inventory.Application.IntegrationEvents;

public sealed class ProductVariantAddedEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public required long ProductId { get; init; }
    public required long VariantId { get; init; }
    public required string Sku { get; init; }
}