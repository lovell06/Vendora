using Scalar.AspNetCore;
using Vendora.Services.Identity.Api;
using Vendora.Services.Identity.Api.Extensions;
using Vendora.Services.Identity.Application;
using Vendora.Services.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapApiEndpoints();

app.Run();