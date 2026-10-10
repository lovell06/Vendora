using Vendora.Services.Cart.Infrastructure.Persistence;
using Vendora.Services.Cart.Infrastructure.Queries;
using Vendora.Services.Cart.Infrastructure.Repositories;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddKeyPerFile("/run/secrets/", optional: true);

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
});

builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddRepositoryServices()
    .AddQueryServices();

builder.Services
    .AddRabbitMqOptions()
    .AddRabbitMqConsumers();

builder.Services.AddEventTypeRegistry();

builder.Services
    .AddTimeProvider()
    .AddApplicationHandlers()
    .AddIntegrationEventHandlers()
    .AddIntegrationEventDispatcher();

var host = builder.Build();
host.Run();