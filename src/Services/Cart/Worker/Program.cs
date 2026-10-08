var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRabbitMqOptions();
builder.Services.AddIntegrationEventHandlers();
builder.Services.AddEventTypeRegistry();
builder.Services.AddIntegrationEventDispatcher();
builder.Services.AddRabbitMqConsumers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
var host = builder.Build();
host.Run();