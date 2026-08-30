using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Identity.Application.Abstractions.Authentication;

namespace Vendora.Services.Identity.Infrastructure.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHashProvider, AspNetCorePasswordHashProvider>();
        
        return services;
    }
}