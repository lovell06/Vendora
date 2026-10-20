namespace Vendora.Services.Ordering.Infrastructure.Clients;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCartServiceClient(IConfiguration configuration)
        {
            services.AddHttpClient<ICartServiceClient, HttpCartServiceClient>(client =>
            {
                client.BaseAddress = new Uri(
                    configuration["Services:Cart:BaseAddress"]
                    ?? throw new InvalidOperationException(
                        "Missing Services:Cart:BaseAddress Configuration."));

                client.Timeout = TimeSpan.FromSeconds(5);
            });
            return services;
        }

        public IServiceCollection AddCatalogServiceClient(IConfiguration configuration)
        {
            services.AddHttpClient<ICatalogServiceClient, HttpCatalogServiceClient>(client =>
            {
                client.BaseAddress = new Uri(
                    configuration["Services:Catalog:BaseAddress"]
                    ?? throw new InvalidOperationException("Missing Services:Catalog:BaseAddress Configuration."));

                client.Timeout = TimeSpan.FromSeconds(5);
            });
            return services;
        }

        public IServiceCollection AddInventoryServiceClient(IConfiguration configuration)
        {
            services.AddHttpClient<IInventoryServiceClient, HttpInventoryServiceClient>(client =>
            {
                client.BaseAddress = new Uri(
                    configuration["Services:Inventory:BaseAddress"]
                    ?? throw new InvalidOperationException("Missing Services:Inventory:BaseAddress Configuration."));

                client.Timeout = TimeSpan.FromSeconds(5);
            });

            return services;
        }
    }
}