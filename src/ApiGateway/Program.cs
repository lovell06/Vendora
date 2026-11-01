using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.AddDocument("identity", "Identity", "/openapi/identity.json")
            .AddDocument("catalog", "Catalog", "/openapi/catalog.json")
            .AddDocument("inventory", "Inventory", "/openapi/inventory.json")
            .AddDocument("cart", "Cart", "/openapi/cart.json")
            .AddDocument("ordering", "Ordering", "/openapi/ordering.json");
    });
}

app.MapHealthChecks("/health");

app.MapReverseProxy();

app.Run();
