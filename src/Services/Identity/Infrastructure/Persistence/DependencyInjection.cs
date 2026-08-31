using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Identity.Application.Abstractions.Persistence;
using Vendora.Services.Identity.Domain.Users;
using Vendora.Services.Identity.Infrastructure.Persistence.Repositories;
using Vendora.Services.Identity.Infrastructure.Persistence.UnitOfWork;

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

        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();

        services.AddScoped<IUserRepository, PostgresUserRepository>();
        
        return services;
    }
}