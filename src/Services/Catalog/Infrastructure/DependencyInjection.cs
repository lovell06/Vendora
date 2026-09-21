using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Catalog.Infrastructure.Persistence;
using Vendora.Services.Catalog.Infrastructure.Queries;
using Vendora.Services.Catalog.Infrastructure.Repositories;

namespace Vendora.Services.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddQueries();
        services.AddRepositories();
        
        return services;
    }
}