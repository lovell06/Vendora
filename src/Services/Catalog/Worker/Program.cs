var builder = Host.CreateApplicationBuilder(args);

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
