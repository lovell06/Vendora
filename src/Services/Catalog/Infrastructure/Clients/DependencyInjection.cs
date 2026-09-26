using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Catalog.Application.Abstractions.Clients.Inventory;

namespace Vendora.Services.Catalog.Infrastructure.Clients;

public static class DependencyInjection
{
    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IInventoryClient, HttpInventoryClient>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["Services:Inventory:BaseUrl"]
                ?? throw new InvalidOperationException(
                    "Missing Services:Inventory:BaseUrl configuration."));

            client.Timeout = TimeSpan.FromSeconds(5);
        });
        return services;
    }
}