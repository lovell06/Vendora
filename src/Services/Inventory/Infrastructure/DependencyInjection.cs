using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Inventory.Infrastructure.Persistence;
using Vendora.Services.Inventory.Infrastructure.Queries;
using Vendora.Services.Inventory.Infrastructure.Repositories;

namespace Vendora.Services.Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddRepositories();
        services.AddQueryServices();

        return services;
    }
}