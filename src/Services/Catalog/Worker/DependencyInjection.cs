using Vendora.BuildingBlocks.Messaging.TypeRegistry;
using Vendora.Services.Catalog.Application.IntegrationEvents;

namespace Vendora.Services.Catalog.Worker;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddEventTypeRegistry()
        {
            services.AddSingleton<IEventTypeRegistry>(_ =>
            {
                var registry = new EventTypeRegistry();

                registry.Add("catalog.product-created", typeof(ProductCreatedEvent));

                return registry;
            });
            return services;
        }

        public IServiceCollection AddEventPublisher()
        {
            return services.AddHostedService<EventPublisher>();
        }
    }
}