using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vendora.Services.Identity.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
                               ?? throw new InvalidOperationException("Postgres connection is not configured.");

        services.AddDbContext<PostgresDbContext>(builder =>
        {
            builder.UseNpgsql(connectionString);
        });
        
        return services;
    }
}