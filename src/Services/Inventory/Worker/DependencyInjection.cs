using Vendora.Services.Inventory.Worker.EventHandlers;

namespace Vendora.Services.Inventory.Worker;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIntegrationEventHandlers()
        {
            services.AddTransient<
                IIntegrationEventHandler<ProductVariantAddedEvent>, 
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
                // registry.Add("catalog.product.created", typeof(ProductCreatedEvent));
                registry.Add("catalog.product-variant.added", typeof(ProductVariantAddedEvent));
                registry.Add("ordering.order.cancelled", typeof(OrderCancelledEvent));

                return registry;
            });

            return services;
        }
    }
}