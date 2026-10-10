using Vendora.BuildingBlocks.Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
});

builder.Services
    .AddTimeProvider()
    .AddPersistenceServices(builder.Configuration)
    .AddRepositoryServices()
    .AddRedisConnection(builder.Configuration);

builder.Services
    .AddSmtpOptions()
    .AddEmailSender()
    .AddEmailVerificationTokenProvider();

builder.Services
    .AddIntegrationEventHandlers()
    .AddIntegrationEventDispatcher();

builder.Services
    .AddEventTypeRegistry()
    .AddRabbitMqOptions()
    .AddRabbitMqConsumerOptions()
    .AddRabbitMqEventBus()
    .AddRabbitMqConsumer();

builder.Services.AddEventPublisher();

var host = builder.Build();
host.Run();
