using Vendora.BuildingBlocks.Messaging.TypeRegistry;
using Vendora.Services.Ordering.Application.IntegrationEvents;

namespace Vendora.Services.Ordering.Worker;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddWorkers()
        {
            services.AddHostedService<OutboxMessageWorker>();
            return services;
        }

        public IServiceCollection AddOrderingEventTypeRegistry()
        {
            services.AddSingleton<IEventTypeRegistry>(_ =>
            {
                var registry = new EventTypeRegistry();
                registry.Add("ordering.order-placed", typeof(OrderPlacedEvent));

                return registry;
            });

            return services;
        }
    }
}