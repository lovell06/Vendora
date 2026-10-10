var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets/", optional: true);

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.UseUtcTimestamp = true;
});

builder.Services
    .AddTimeProvider()
    .AddApplicationHandlers();

builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddQueryServices()
    .AddRepositoryServices();

builder.Services
    .AddIntegrationEventHandlers()
    .AddIntegrationEventDispatcher();

builder.Services.AddEventTypeRegistry();

builder.Services
    .AddRabbitMqOptions()
    .AddRabbitMqConsumerOptions()
    .AddRabbitMqConsumer();

var host = builder.Build();
host.Run();