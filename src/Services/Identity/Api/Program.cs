using Vendora.Services.Identity.Api.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddOpenApi();

builder.Services
    .AddTimeProvider()
    .AddApplicationHandlers();

builder.Services
    .AddAuthenticationServices()
    .AddEmailServices()
    .AddInfrastructureOptions()
    .AddPersistenceServices(builder.Configuration)
    .AddRedisConnection(builder.Configuration)
    .AddRepositoryServices()
    .AddEventTypeRegistry();

builder.Services
    .AddJwtBearerAuthentication(builder.Configuration)
    .AddApplicationAuthorization();

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.UseUtcTimestamp = true;
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ConflictExceptionHandler>();
builder.Services.AddExceptionHandler<UnhandledExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

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

app.MapHealthChecks("/health").AllowAnonymous();

app.MapApiEndpoints();

app.Run();