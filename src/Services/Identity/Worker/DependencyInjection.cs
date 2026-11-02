using Vendora.Services.Identity.Worker.EventHandlers;

namespace Vendora.Services.Identity.Worker;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIntegrationEventHandlers()
        {
            services.AddTransient<
                IIntegrationEventHandler<UserRegisteredEvent>, 
                SendVerificationEmailOnUserRegisteredHandler>();

            return services;
        }

        public IServiceCollection AddEventTypeRegistry()
        {
            services.AddSingleton<IEventTypeRegistry>(_ =>
            {
                var eventTypeRegistry = new EventTypeRegistry();

                eventTypeRegistry.Add("identity.user-registered", typeof(UserRegisteredEvent));

                return eventTypeRegistry;
            });
            return services;
        }

        public IServiceCollection AddOutboxMessageWorker()
        {
            return services.AddHostedService<OutboxMessageWorker>();
        }
    }
}