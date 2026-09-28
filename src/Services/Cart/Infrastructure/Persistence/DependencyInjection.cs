using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vendora.Services.Cart.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
                               ?? throw new InvalidOperationException(
                                   "PostgreSQL connection string is not configured.");
        
        services.AddDbContext<PostgresDbContext>(builder =>
        {
            builder.UseNpgsql(connectionString);
        });
        return services;
    }
}