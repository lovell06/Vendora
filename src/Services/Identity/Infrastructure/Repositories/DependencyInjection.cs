using Microsoft.Extensions.DependencyInjection;
using Vendora.Services.Identity.Domain.Users;

namespace Vendora.Services.Identity.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, PostgresUserRepository>();
        return services;
    }
}