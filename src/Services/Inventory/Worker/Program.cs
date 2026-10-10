using Vendora.Services.Inventory.Infrastructure.Queries;

var builder = Host.CreateApplicationBuilder(args);

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