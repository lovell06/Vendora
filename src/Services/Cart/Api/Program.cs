using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Vendora.Services.Cart.Api;
using Vendora.Services.Cart.Api.Extensions;
using Vendora.Services.Cart.Application;
using Vendora.Services.Cart.Infrastructure;
using Vendora.Services.Cart.Infrastructure.Persistence;

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

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();