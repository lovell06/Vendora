namespace Vendora.Services.Cart.Worker;

public static class DependencyInjection
{
    public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
    {
        services.AddTransient<
            IIntegrationEventHandler<UserRegisteredEvent>,
            InitializeCartIntegrationEventHandler>();
        return services;
    }

    public static IServiceCollection AddEventTypeRegistry(this IServiceCollection services)
    {
        services.AddSingleton<IEventTypeRegistry>(_ =>
        {
            var registry = new EventTypeRegistry();
            registry.Add("identity.user-registered", typeof(UserRegisteredEvent));

            return registry;
        });
        return services;
    }

    public static IServiceCollection AddRabbitMqOptions(this IServiceCollection services)
    {
        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration(RabbitMqOptions.SectionName)
            .ValidateOnStart();

        services.AddOptions<RabbitMqConsumerOptions>()
            .BindConfiguration(RabbitMqConsumerOptions.SectionName)
            .ValidateOnStart();
        
        return services;
    }

    public static IServiceCollection AddIntegrationEventDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
        return services;
    }

    public static IServiceCollection AddRabbitMqConsumers(this IServiceCollection services)
    {
        services.AddHostedService<RabbitMqConsumer>();
        return services;
    }
}