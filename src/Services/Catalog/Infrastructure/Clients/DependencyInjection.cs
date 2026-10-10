namespace Vendora.Services.Catalog.Infrastructure.Clients;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServiceClients(IConfiguration configuration)
        {
            services.AddInventoryServiceClient(configuration);
            return services;
        }

        public IServiceCollection AddInventoryServiceClient(IConfiguration configuration)
        {
            services.AddHttpClient<IInventoryClient, HttpInventoryClient>(client =>
            {
                client.BaseAddress = new Uri(
                    configuration["Services:Inventory:BaseUrl"]
                    ?? throw new InvalidOperationException(
                        "Missing Services:Inventory:BaseUrl configuration."));

                client.Timeout = TimeSpan.FromSeconds(5);
            });

            return services;
        }
    }
}