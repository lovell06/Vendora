using Vendora.Services.Identity.Infrastructure.Options;
using Vendora.Services.Identity.Infrastructure.Redis;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
});

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddOptions<SmtpOptions>()
    .BindConfiguration(SmtpOptions.SectionName)
    .ValidateOnStart();

builder.Services.AddEventTypeRegistry();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddRepositoryServices();
builder.Services.AddRedisConnection(builder.Configuration);
builder.Services.AddInfrastructureEmail();
builder.Services.AddIntegrationEventHandlers();
builder.Services.AddEventBus(builder.Configuration);
builder.Services.AddHostedService<OutboxWorker>();
builder.Services.AddEventConsumer();
builder.Services.AddIntegrationEventDispatcher();

var host = builder.Build();
host.Run();
