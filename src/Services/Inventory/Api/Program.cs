using Scalar.AspNetCore;
using Vendora.Services.Inventory.Api;
using Vendora.Services.Inventory.Api.Extensions;
using Vendora.Services.Inventory.Application;
using Vendora.Services.Inventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfastructure(builder.Configuration);
builder.Services.AddApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();