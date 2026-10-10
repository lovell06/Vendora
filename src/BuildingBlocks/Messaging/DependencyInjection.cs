using Vendora.BuildingBlocks.Messaging.Dispatching;
using Vendora.BuildingBlocks.Messaging.RabbitMq;

namespace Vendora.BuildingBlocks.Messaging;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRabbitMqOptions()
        {
            services.AddOptions<RabbitMqOptions>()
                .BindConfiguration(RabbitMqOptions.SectionName)
                .ValidateOnStart();

            return services;
        }

        public IServiceCollection AddRabbitMqConsumerOptions()
        {
            services.AddOptions<RabbitMqConsumerOptions>()
                .BindConfiguration(RabbitMqConsumerOptions.SectionName)
                .ValidateOnStart();

            return services;
        }

        public IServiceCollection AddIntegrationEventDispatcher()
        {
            return services.AddScoped<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
        }

        public IServiceCollection AddRabbitMqEventBus()
        {
            return services.AddSingleton<IEventBus, RabbitMqEventBus>();
        }

        public IServiceCollection AddRabbitMqConsumer()
        {
            return services.AddHostedService<RabbitMqConsumer>();
        }
    }
}