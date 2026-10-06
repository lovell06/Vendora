namespace Vendora.BuildingBlocks.Messaging.RabbitMq;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public required string Host { get; init; }
    public int Port { get; init; } = 5672;
    public required string VirtualHost { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string Exchange { get; init; }
    public int PublishTimeoutSeconds { get; init; } = 30;
}