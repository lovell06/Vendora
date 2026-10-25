namespace Vendora.Services.Ordering.Worker;

public class OutboxMessageWorker(
    IEventBus eventBus,
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxMessageWorker> logger,
    TimeProvider clock
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            var utcNow = clock.GetUtcNow();

            await using var scope = scopeFactory.CreateAsyncScope();

            var outboxMessageRepository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var outboxMessages =  await outboxMessageRepository.ListAsync(10, stoppingToken);
            foreach (var outboxMessage in outboxMessages)
            {
                try
                {
                    await eventBus.PublishAsync(new IntegrationMessage
                    {
                        Id = outboxMessage.Id,
                        Type = outboxMessage.Type,
                        Payload = outboxMessage.Payload
                    }, stoppingToken);

                    outboxMessage.ProcessedAt = utcNow;
                }
                catch (Exception ex)
                {
                    outboxMessage.NextAttemptAt = utcNow.AddSeconds(10);
                    outboxMessage.LastError = ex.Message;
                }
                finally
                {
                    await unitOfWork.SaveChangesAsync(stoppingToken);
                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
