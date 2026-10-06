namespace Vendora.BuildingBlocks.Messaging.Messages;

public sealed class IntegrationMessage
{
    public Guid Id { get; init; }
    public required string Type { get; init; }
    public required string Payload { get; init; }
}