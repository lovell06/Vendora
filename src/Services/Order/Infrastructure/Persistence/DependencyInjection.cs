using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Order.Application.Abstractions.Persistence;

namespace Vendora.Services.Order.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostgresDbContext>(options =>
        {
            var conn = configuration.GetConnectionString("Postgres")
                       ?? throw new InvalidOperationException("Postgres connection string is not configured.");

            options.UseNpgsql(conn);
        });

        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

        return services;
    }
}