using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Vendora.BuildingBlocks.Messaging.RabbitMq;

public sealed class RabbitMqConsumer(
    IOptions<RabbitMqOptions> rabbitOptions,
    IOptions<RabbitMqConsumerOptions> consumerOptions,
    IServiceScopeFactory scopeFactory,
    ILogger<RabbitMqConsumer> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        try
        {
            await ConsumeAsync(stoppingToken);
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            // Worker đang dừng theo yêu cầu.
        }
    }

    private async Task ConsumeAsync(CancellationToken ct)
    {
        var rabbit = rabbitOptions.Value;
        var subscription = consumerOptions.Value;

        // 1. Chuẩn bị cấu hình kết nối.
        var factory = new ConnectionFactory
        {
            HostName = rabbit.Host,
            Port = rabbit.Port,
            VirtualHost = rabbit.VirtualHost,
            UserName = rabbit.UserName,
            Password = rabbit.Password,

            AutomaticRecoveryEnabled = false,
            ConsumerDispatchConcurrency = 1
        };

        // 2. Mở connection và channel.
        await using var connection =
            await factory.CreateConnectionAsync(ct);

        await using var channel =
            await connection.CreateChannelAsync(
                cancellationToken: ct);

        // 3. Bảo đảm exchange tồn tại.
        await channel.ExchangeDeclareAsync(
            exchange: rabbit.Exchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: ct);

        // 4. Bảo đảm queue tồn tại.
        await channel.QueueDeclareAsync(
            queue: subscription.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: ct);

        // 5. Nối những routing key cần nhận vào queue này.
        foreach (var bindingKey in subscription.BindingKeys)
        {
            await channel.QueueBindAsync(
                queue: subscription.QueueName,
                exchange: rabbit.Exchange,
                routingKey: bindingKey,
                cancellationToken: ct);
        }

        // 6. Giới hạn số message chưa ACK.
        await channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: subscription.PrefetchCount,
            global: false,
            cancellationToken: ct);

        // 7. Khai báo cách xử lý mỗi message.
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, delivery) =>
        {
            var receivingChannel = ((AsyncEventingBasicConsumer)sender).Channel;
            try
            {
                var eventName = delivery.BasicProperties.Type;

                if (string.IsNullOrWhiteSpace(eventName))
                {
                    throw new InvalidOperationException(
                        "Message is missing its Type property.");
                }

                await using var scope =
                    scopeFactory.CreateAsyncScope();

                var dispatcher = scope.ServiceProvider
                    .GetRequiredService<IIntegrationEventDispatcher>();

                await dispatcher.DispatchAsync(
                    eventName,
                    delivery.Body,
                    ct);
            }
            catch (OperationCanceledException)
                when (ct.IsCancellationRequested)
            {
                // Không ACK khi xử lý bị hủy.
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Handle event failed. MessageId={MessageId}, Type={Type}",
                    delivery.BasicProperties.MessageId,
                    delivery.BasicProperties.Type);

                try
                {
                    // Chờ trước khi trả message về queue.
                    await Task.Delay(TimeSpan.FromSeconds(5), ct);

                    await receivingChannel.BasicNackAsync(
                        deliveryTag: delivery.DeliveryTag,
                        multiple: false,
                        requeue: true,
                        cancellationToken: ct);
                }
                catch (OperationCanceledException)
                    when (ct.IsCancellationRequested)
                {
                    // Connection đóng khi Worker dừng sẽ requeue
                    // message chưa được xác nhận.
                }

                return;
            }

            // Chỉ ACK khi dispatcher hoàn thành thành công.
            await receivingChannel.BasicAckAsync(
                deliveryTag: delivery.DeliveryTag,
                multiple: false,
                cancellationToken: ct);
        };

        // Ghi log nếu callback gặp lỗi ngoài phần xử lý bên trên,
        // chẳng hạn lỗi khi gửi ACK/NACK.
        channel.CallbackExceptionAsync += (_, args) =>
        {
            logger.LogError(
                args.Exception,
                "Consumer callback failed.");

            return Task.CompletedTask;
        };

        // 8. Bắt đầu đăng ký nhận message.
        await channel.BasicConsumeAsync(
            queue: subscription.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: ct);

        logger.LogInformation(
            "Consumer started. Queue={QueueName}",
            subscription.QueueName);

        // 9. Giữ connection và channel sống.
        while (connection.IsOpen && channel.IsOpen)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), ct);
        }

        logger.LogWarning("Consumer connection or channel closed.");
    }
}