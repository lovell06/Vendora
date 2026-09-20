using Scalar.AspNetCore;
using Vendora.Services.Catalog.Api;
using Vendora.Services.Catalog.Api.Extensions;
using Vendora.Services.Catalog.Application;
using Vendora.Services.Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

builder.Logging.AddSimpleConsole(config =>
{
    config.SingleLine = true;
    config.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    config.UseUtcTimestamp = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.UseHttpsRedirection();

app.Run();