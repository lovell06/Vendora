using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Cart.Infrastructure.Authentication;
using Vendora.Services.Cart.Infrastructure.Persistence;
using Vendora.Services.Cart.Infrastructure.Queries;
using Vendora.Services.Cart.Infrastructure.Repositories;

namespace Vendora.Services.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationServices();
        services.AddPersistence(configuration);
        services.AddRepositories();
        services.AddQueryServices();
        
        return services;
    }
}