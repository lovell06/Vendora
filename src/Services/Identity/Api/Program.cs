var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddApplicationHandlers()
    .AddAuthenticationServices()
    .AddServiceClients(builder.Configuration)
    .AddEmailServices()
    .AddInfrastructureOptions()
    .AddPersistenceServices(builder.Configuration)
    .AddRedisConnection(builder.Configuration)
    .AddRepositoryServices()
    .AddEventTypeRegistry()
    .AddJwtBearerAuthentication(builder.Configuration)
    .AddAuthorization();


builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.UseUtcTimestamp = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();