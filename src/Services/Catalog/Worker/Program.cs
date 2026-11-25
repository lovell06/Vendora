var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets/", optional: true);

builder.Logging.AddSimpleConsole(options =>
{
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
});

builder.Services
    .AddTimeProvider()
    .AddPersistenceServices(builder.Configuration)
    .AddRepositoryServices();

builder.Services.AddEventTypeRegistry();

builder.Services
    .AddRabbitMqOptions()
    .AddRabbitMqEventBus();

builder.Services.AddEventPublisher();

var host = builder.Build();
host.Run();
