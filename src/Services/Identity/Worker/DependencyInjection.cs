using Vendora.BuildingBlocks.Messaging.Dispatching;

namespace Vendora.Services.Identity.Worker;

public static class DependencyInjection
{
    public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
    {
        services.AddTransient<IIntegrationEventHandler<UserRegisteredEvent>, SendVerificationEmailHandler>();
        
        return services;
    }

    public static IServiceCollection AddEventTypeRegistry(this IServiceCollection services)
    {
        services.AddSingleton<IEventTypeRegistry>(_ =>
        {
            var eventTypeRegistry = new EventTypeRegistry();

            eventTypeRegistry.Add("identity.user-registered", typeof(UserRegisteredEvent));

            return eventTypeRegistry;
        });
        return services;
    }

    public static IServiceCollection AddEventBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration(RabbitMqOptions.SectionName)
            .Validate(
                options => options.PublishTimeoutSeconds > 0,
                "PublishTimeoutSeconds must be more than or equal zero.")
            .ValidateOnStart();

        services.AddSingleton<IEventBus, RabbitMqEventBus>();

        return services;
    }

    public static IServiceCollection AddEventConsumer(
        this IServiceCollection services)
    {
        services.AddOptions<RabbitMqConsumerOptions>()
            .BindConfiguration(RabbitMqConsumerOptions.SectionName)
            .Validate(
                options => options.PrefetchCount > 0,
                "Prefetch count must be positive number.")
            .ValidateOnStart();

        services.AddHostedService<RabbitMqConsumer>();
        return services;
    }

    public static IServiceCollection AddIntegrationEventDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
        return services;
    }
}