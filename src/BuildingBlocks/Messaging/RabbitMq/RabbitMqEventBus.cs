using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Vendora.BuildingBlocks.Messaging.RabbitMq;

public sealed class RabbitMqEventBus : IEventBus, IAsyncDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly IConnectionFactory _connectionFactory;
    private readonly SemaphoreSlim _gate = new(1,1);

    private IConnection? _connection;
    private IChannel? _channel;
    private bool _disposed;

    public RabbitMqEventBus(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
        _connectionFactory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            VirtualHost = _options.VirtualHost,
            UserName = _options.UserName,
            Password = _options.Password,
            AutomaticRecoveryEnabled = true,
            TopologyRecoveryEnabled = true
        };
    }

    public async Task PublishAsync(IntegrationMessage message, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentException.ThrowIfNullOrWhiteSpace(message.Type);

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeout.CancelAfter(TimeSpan.FromSeconds(_options.PublishTimeoutSeconds));
        var cancellationToken = timeout.Token;

        await _gate.WaitAsync(cancellationToken);

        try
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            var channel = await GetChannelAsync(cancellationToken);

            var properties = new BasicProperties
            {
                MessageId = message.Id.ToString(),
                Type = message.Type,
                ContentType = "application/json",
                ContentEncoding = "utf-8",
                Persistent = true
            };

            var body = JsonSerializer.SerializeToUtf8Bytes(message);

            await channel.BasicPublishAsync(
                exchange: _options.Exchange,
                routingKey: message.Type,
                mandatory: true,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task<IChannel> GetChannelAsync(CancellationToken ct)
    {
        _connection ??= await _connectionFactory.CreateConnectionAsync(ct);

        if (!_connection.IsOpen)
            throw new IOException("RabbitMQ connection is not ready. Retry after please.");

        if (_channel is { IsOpen: true })
            return _channel;
        
        if (_channel is not null)
        {
            var previousChannel = _channel;
            _channel = null;

            await previousChannel.DisposeAsync();
        }

        var channel = await _connection.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true),
            cancellationToken: ct);

        try
        {
            await channel.ExchangeDeclareAsync(
                exchange: _options.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                cancellationToken: ct);

            _channel = channel;
            return channel;
        }
        catch
        {
            await channel.DisposeAsync();
            throw;
        }
    }

    public Task SubscribeAsync<TEvent, THandler>(CancellationToken ct) where TEvent : IIntegrationEvent where THandler : IIntegrationEventHandler<TEvent>
    {
        throw new NotImplementedException();
    }

    public Task UnsubscribeAsync<TEvent, THandler>(CancellationToken ct) where TEvent : IIntegrationEvent where THandler : IIntegrationEventHandler<TEvent>
    {
        throw new NotImplementedException();
    }

    public async ValueTask DisposeAsync()
    {
        await _gate.WaitAsync();

        try
        {
            if (_disposed)
                return;

            _disposed = true;


            try
            {
                if (_channel is not null)
                    await _channel.DisposeAsync();
            }
            finally
            {
                if (_connection is not null)
                    await _connection.DisposeAsync();
            }
        }
        finally
        {
            _gate.Release();
        }
    }
}