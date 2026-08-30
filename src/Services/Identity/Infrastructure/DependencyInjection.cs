using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Identity.Infrastructure.Authentication;
using Vendora.Services.Identity.Infrastructure.Email;
using Vendora.Services.Identity.Infrastructure.Options;
using Vendora.Services.Identity.Infrastructure.Redis;

namespace Vendora.Services.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureAuthentication();
        services.AddInfrastructureEmail();
        services.AddInfrastructureOptions();
        services.AddRedisConnection(configuration);
        
        return services;
    }
}