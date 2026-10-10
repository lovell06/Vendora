namespace Vendora.Services.Inventory.Worker;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIntegrationEventHandlers()
        {
            services.AddTransient<
                IIntegrationEventHandler<ProductCreatedEvent>, 
                InitializeInventoryHandler>();
            
            return services;
        }

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
    }
}