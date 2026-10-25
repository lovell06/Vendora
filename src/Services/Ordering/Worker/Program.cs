var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets/", optional: true);

builder.Logging.AddSimpleConsole(options =>
{
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.SingleLine = true;
});

builder.Services
    .AddTimeProvider()
    .AddApplicationHandlers();
    
builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddRepositoryServices();

builder.Services
    .AddOrderingEventTypeRegistry()
    .AddRabbitMqOptions()
    .AddRabbitMqEventBus();

builder.Services
    .AddCartServiceClient(builder.Configuration)
    .AddCatalogServiceClient(builder.Configuration)
    .AddInventoryServiceClient(builder.Configuration);

builder.Services
    .AddWorkers();

var host = builder.Build();
host.Run();