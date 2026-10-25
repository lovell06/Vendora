namespace Vendora.Services.Cart.Worker;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIntegrationEventHandlers()
        {
            services.AddTransient<
                IIntegrationEventHandler<InitializeCartIntegrationEvent>,
                InitializeCartIntegrationEventHandler>();

            services.AddTransient<
                IIntegrationEventHandler<RemoveOrderedItemsIntegrationEvent>,
                RemoveOrderedItemsIntegrationEventHandler>();
            
            return services;
        }

        public IServiceCollection AddEventTypeRegistry()
        {
            services.AddSingleton<IEventTypeRegistry>(_ =>
            {
                var registry = new EventTypeRegistry();
                registry.Add("identity.user-registered", typeof(InitializeCartIntegrationEvent));
                registry.Add("ordering.order-placed", typeof(RemoveOrderedItemsIntegrationEvent));

                return registry;
            });
            return services;
        }

        public IServiceCollection AddRabbitMqOptions()
        {
            services.AddOptions<RabbitMqOptions>()
                .BindConfiguration(RabbitMqOptions.SectionName)
                .ValidateOnStart();

            services.AddOptions<RabbitMqConsumerOptions>()
                .BindConfiguration(RabbitMqConsumerOptions.SectionName)
                .ValidateOnStart();
        
            return services;
        }

        public IServiceCollection AddIntegrationEventDispatcher()
        {
            services.AddScoped<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
            return services;
        }

        public IServiceCollection AddRabbitMqConsumers()
        {
            services.AddHostedService<RabbitMqConsumer>();
            return services;
        }
    }
}