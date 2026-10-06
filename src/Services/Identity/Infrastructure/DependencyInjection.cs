using Vendora.BuildingBlocks.Messaging.TypeRegistry;
using Vendora.Services.Identity.Application.Authentication.Register.Events;

namespace Vendora.Services.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IEventTypeRegistry>(_ =>
        {
            var mapper = new EventTypeRegistry();
            mapper.Add("identity.user-registered", typeof(UserRegisteredEvent));
            return mapper;
        });

        services.AddRedisConnection(configuration);
        services.AddPersistence(configuration);
        services.AddInfrastructureAuthentication();
        services.AddInfrastructureEmail();
        services.AddInfrastructureOptions();
        services.AddRepositoryServices();
        services.AddClients(configuration);

        return services;
    }
}
