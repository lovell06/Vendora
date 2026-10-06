namespace Vendora.Services.Identity.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, EfCoreUserRepository>();
        services.AddScoped<IOutboxRepository, EfCoreOutboxRepository>();
        return services;
    }
}