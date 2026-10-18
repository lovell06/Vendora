using Vendora.Services.Cart.Infrastructure.Queries;
using Vendora.Services.Cart.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
});

builder.Services
    .AddTimeProvider()
    .AddApplicationHandlers();

builder.Services
    .AddCurrentUserService()
    .AddPersistenceServices(builder.Configuration)
    .AddRepositoryServices()
    .AddQueryServices();

builder.Services
    .AddJwtBearerAuthentication(builder.Configuration)
    .AddApplicationAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();