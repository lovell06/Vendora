var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

builder.Services.AddOpenApi();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services
    .AddTimeProvider()
    .AddApplicationHandlers();

builder.Services
    .AddCurrentUserService()
    .AddServiceClients(builder.Configuration)
    .AddPersistenceServices(builder.Configuration)
    .AddQueryServices()
    .AddRepositoryServices()
    .AddEventTypeRegistry();

builder.Services
    .AddJwtBearerAuthentication(builder.Configuration)
    .AddApplicationAuthorization();

builder.Logging.AddSimpleConsole(config =>
{
    config.SingleLine = true;
    config.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    config.UseUtcTimestamp = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await using var scope = app.Services.CreateAsyncScope();

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await db.Database.MigrateAsync();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.UseHttpsRedirection();

app.Run();