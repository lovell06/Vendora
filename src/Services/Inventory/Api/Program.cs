using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Vendora.Services.Inventory.Api;
using Vendora.Services.Inventory.Api.Extensions;
using Vendora.Services.Inventory.Application;
using Vendora.Services.Inventory.Infrastructure;
using Vendora.Services.Inventory.Infrastructure.Persistence;

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
builder.Services.AddInfastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await using var scope = app.Services.CreateAsyncScope();

    var db = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();

    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();