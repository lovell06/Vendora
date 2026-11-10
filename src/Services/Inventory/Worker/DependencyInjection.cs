using Vendora.Services.Inventory.Worker.EventHandlers;

namespace Vendora.Services.Inventory.Worker;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIntegrationEventHandlers()
        {
            services.AddTransient<
                IIntegrationEventHandler<ProductCreatedEvent>, 
                InitializeOnProductCreatedHandler>();

            services.AddTransient<
                IIntegrationEventHandler<OrderCancelledEvent>,
                ReleaseStockOnOrderCancelledHandler>();
            
            return services;
        }

        public IServiceCollection AddEventTypeRegistry()
        {
            services.AddSingleton<IEventTypeRegistry>(_ =>
            {
                var registry = new EventTypeRegistry();
                registry.Add("catalog.product.created", typeof(ProductCreatedEvent));
                registry.Add("ordering.order.cancelled", typeof(OrderCancelledEvent));

                return registry;
            });

            return services;
        }
    }
}