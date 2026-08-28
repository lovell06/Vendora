using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Vendora.Services.Identity.Infrastructure.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRedisConnection(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var connString = configuration
                                 .GetConnectionString("Redis") 
                             ?? throw new InvalidOperationException("Redis connection is not configured.");

            return ConnectionMultiplexer.Connect(connString);
        });

        return services;
    }
}