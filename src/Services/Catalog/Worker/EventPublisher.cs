namespace Vendora.Services.Catalog.Worker;

public class EventPublisher(
    IEventBus eventBus,
    IServiceScopeFactory scopeFactory,
    TimeProvider clock,
    ILogger<EventPublisher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await using var scope = scopeFactory.CreateAsyncScope();
            var provider = scope.ServiceProvider;

            var outboxRepository = provider.GetRequiredService<IOutboxRepository>();

            var messages = await outboxRepository.ListAsync(50, stoppingToken);

            foreach (var message in messages)
            {
                await eventBus.PublishAsync(new IntegrationMessage
                {
                    Id = message.Id,
                    Payload = message.Payload,
                    Type = message.Type
                }, stoppingToken);

                message.ProcessedAt = clock.GetUtcNow();
            }

            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.SaveChangesAsync(stoppingToken);
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}
