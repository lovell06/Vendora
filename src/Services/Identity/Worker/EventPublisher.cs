using Vendora.BuildingBlocks.Messaging.Messages;

namespace Vendora.Services.Identity.Worker;

public class EventPublisher(
    ILogger<EventPublisher> logger,
    IServiceScopeFactory scopeFactory,
    IEventBus eventBus,
    TimeProvider clock) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            using var scope = scopeFactory.CreateScope();

            var provider = scope.ServiceProvider;

            var outboxRepository = provider.GetRequiredService<IOutboxRepository>();

            var messages = await outboxRepository.ListAsync(10, stoppingToken);

            foreach (var message in messages)
            {
                await eventBus.PublishAsync(new IntegrationMessage
                {
                    Id = message.Id,
                    Type = message.Type,
                    Payload = message.Payload,
                }, stoppingToken);

                message.ProcessedAt = clock.GetUtcNow();
            }

            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.SaveChangesAsync(stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
