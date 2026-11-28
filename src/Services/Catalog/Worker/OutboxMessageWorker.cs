namespace Vendora.Services.Catalog.Worker;

public class OutboxMessageWorker(
    IEventBus eventBus,
    IServiceScopeFactory scopeFactory,
    TimeProvider clock,
    ILogger<OutboxMessageWorker> logger) : BackgroundService
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

            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();

            foreach (var message in messages)
            {
                try
                {
                    await eventBus.PublishAsync(new IntegrationMessage
                    {
                        Id = message.Id,
                        Payload = message.Payload,
                        Type = message.Type
                    }, stoppingToken);

                    message.ProcessedAt = clock.GetUtcNow();
                }
                catch (Exception e)
                {
                    message.NextAttemptAt = clock.GetUtcNow().AddSeconds(10);
                    message.LastError = e.Message;
                }
                finally
                {
                    ++message.AttemptCount;
                }
            }

            await unitOfWork.SaveChangesAsync(stoppingToken);
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}
