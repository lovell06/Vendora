using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Cart.Application.Abstractions.Persistence;

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

        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();
        return services;
    }
}