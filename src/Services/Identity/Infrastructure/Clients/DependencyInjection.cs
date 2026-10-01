using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Identity.Application.Abstractions.Clients.Cart;

namespace Vendora.Services.Identity.Infrastructure.Clients;

public static class DependencyInjection
{
    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICartClient, HttpCartClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:Cart:BaseUrl"]
                                         ?? throw new InvalidOperationException("Missing Services:Cart:BaseUrl"));

            client.Timeout = TimeSpan.FromSeconds(5);
        });
        return services;
    }
}
