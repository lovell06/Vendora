using Microsoft.EntityFrameworkCore;
using Vendora.Services.Ordering.Infrastructure.Clients;
using Vendora.Services.Ordering.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
    .AddCurrentUserService()
    .AddPersistenceServices(builder.Configuration)
    .AddRepositoryServices();

builder.Services
    .AddOrderingEventTypeRegistry();

builder.Services
    .AddCartServiceClient(builder.Configuration)
    .AddCatalogServiceClient(builder.Configuration)
    .AddInventoryServiceClient(builder.Configuration);

builder.Services
    .AddJwtBearerAuthentication(builder.Configuration)
    .AddApplicationAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health").AllowAnonymous();

app.MapEndpoints();

app.Run();