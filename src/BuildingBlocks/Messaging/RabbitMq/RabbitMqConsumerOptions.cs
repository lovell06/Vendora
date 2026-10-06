namespace Vendora.BuildingBlocks.Messaging.RabbitMq;

public sealed class RabbitMqConsumerOptions
{
    public const string SectionName = "RabbitMqConsumer";

    public required string QueueName { get; init; }
    public string[] BindingKeys { get; init; } = [];
    public ushort PrefetchCount { get; init; } = 1;
}